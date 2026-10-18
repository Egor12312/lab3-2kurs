using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Laboratornaya1 {
  class GraphBuilder {
    private const double zeroValue = 0;
    private const int axisThickness = 1;
    private const int curveThickness = 3;
    private const int startIndex = 0;
    private const int rootDotRadius = 6;
    private const int rootDotDiameter = 12;
    private const int paddingPercent = 10;
    private const int maxPixelLimit = 10000;
    private const double maxGraphValue = 1000000;

    private const double xExpandFactor = 2.0;
    private const double yToleranceFactor = 3.0;
    private const int trimScanSteps = 200;

    public void Build(
        PictureBox pictureBox,
        string formula,
        double startPoint,
        double endPoint,
        FunctionCalculator functionCalculator)
    {
      BuildInternal(pictureBox, formula, startPoint, endPoint, functionCalculator, false, zeroValue, zeroValue);
    }

    public void BuildWithRoot(
        PictureBox pictureBox,
        string formula,
        double startPoint,
        double endPoint,
        FunctionCalculator functionCalculator,
        double rootX,
        double rootY)
    {
      BuildInternal(pictureBox, formula, startPoint, endPoint, functionCalculator, true, rootX, rootY);
    }

    private void BuildInternal(
        PictureBox pictureBox,
        string formula,
        double startPoint,
        double endPoint,
        FunctionCalculator functionCalculator,
        bool showRoot,
        double rootX,
        double rootY)
    {
      int imageWidth = pictureBox.Width;
      int imageHeight = pictureBox.Height;
      if (imageWidth <= startIndex || imageHeight <= startIndex) return;

      double graphStartX;
      double graphEndX;
      ComputeVisibleXRange(
          formula, startPoint, endPoint, functionCalculator,
          out graphStartX, out graphEndX
      );

      Bitmap bitmap = new Bitmap(imageWidth, imageHeight);
      Graphics graphics = Graphics.FromImage(bitmap);
      graphics.SmoothingMode = SmoothingMode.AntiAlias;
      graphics.FillRectangle(Brushes.White, startIndex, startIndex, imageWidth, imageHeight);

      Pen axisPen = new Pen(Color.Black, axisThickness);
      Pen curvePen = new Pen(Color.Blue, curveThickness);
      Pen intervalPen = new Pen(Color.Gray, 1) { DashStyle = DashStyle.Dash };

      double minY = double.MaxValue;
      double maxY = double.MinValue;
      int scanPoints = 500;
      double step = (graphEndX - graphStartX) / scanPoints;

      for (int i = startIndex; i <= scanPoints; ++i)
      {
        double currentX = graphStartX + i * step;
        try
        {
          double currentY = functionCalculator.Calculate(formula, currentX);
          if (double.IsNaN(currentY) == false && double.IsInfinity(currentY) == false)
          {
            if (currentY < minY) minY = currentY;
            if (currentY > maxY) maxY = currentY;
          }
        }
        catch { }
      }

      if (minY == double.MaxValue || maxY == double.MinValue)
      {
        minY = -10; maxY = 10;
      }

      if (double.IsNaN(maxY) || double.IsInfinity(maxY) || maxY > maxGraphValue) maxY = maxGraphValue;
      if (double.IsNaN(minY) || double.IsInfinity(minY) || minY < -maxGraphValue) minY = -maxGraphValue;
      if (maxY - minY < 0.0001) maxY += 1;

      double yPadding = (maxY - minY) * (paddingPercent / 100.0);
      minY -= yPadding;
      maxY += yPadding;

      if (minY > zeroValue) minY = zeroValue - yPadding;
      if (maxY < zeroValue) maxY = zeroValue + yPadding;

      double scaleX = imageWidth / (graphEndX - graphStartX);
      double scaleY = imageHeight / (maxY - minY);

      int axisX = SafeDoubleToInt((zeroValue - graphStartX) * scaleX);
      int axisY = SafeDoubleToInt((maxY - zeroValue) * scaleY);

      if (axisX >= startIndex && axisX <= imageWidth)
        graphics.DrawLine(axisPen, axisX, startIndex, axisX, imageHeight);

      if (axisY >= startIndex && axisY <= imageHeight)
        graphics.DrawLine(axisPen, startIndex, axisY, imageWidth, axisY);

      int axPixel = SafeDoubleToInt((startPoint - graphStartX) * scaleX);
      int bxPixel = SafeDoubleToInt((endPoint - graphStartX) * scaleX);

      if (axPixel >= startIndex && axPixel <= imageWidth)
        graphics.DrawLine(intervalPen, axPixel, startIndex, axPixel, imageHeight);

      if (bxPixel >= startIndex && bxPixel <= imageWidth)
        graphics.DrawLine(intervalPen, bxPixel, startIndex, bxPixel, imageHeight);

      int pointsCount = imageWidth * 2;
      Point[] points = new Point[pointsCount];
      int actualPointsCount = 0;
      bool previousPointValid = false;

      for (int i = startIndex; i < pointsCount; ++i)
      {
        double currentX = graphStartX + (graphEndX - graphStartX) * i / (pointsCount - 1);
        try
        {
          double currentY = functionCalculator.Calculate(formula, currentX);

          if (double.IsNaN(currentY) == false && double.IsInfinity(currentY) == false)
          {
            double calcX = (currentX - graphStartX) * scaleX;
            double calcY = (maxY - currentY) * scaleY;

            points[actualPointsCount] = new Point(SafeDoubleToInt(calcX), SafeDoubleToInt(calcY));
            actualPointsCount++;
            previousPointValid = true;
          }
          else
          {
            FlushPoints(graphics, curvePen, points, actualPointsCount);
            actualPointsCount = 0;
            previousPointValid = false;
          }
        }
        catch
        {
          FlushPoints(graphics, curvePen, points, actualPointsCount);
          actualPointsCount = 0;
          previousPointValid = false;
        }
      }

      FlushPoints(graphics, curvePen, points, actualPointsCount);

      if (showRoot == true)
      {
        int rootPixelX = SafeDoubleToInt((rootX - graphStartX) * scaleX);
        int rootPixelY = SafeDoubleToInt((maxY - rootY) * scaleY);

        int rootDotLeft = rootPixelX - rootDotRadius;
        int rootDotTop = rootPixelY - rootDotRadius;

        graphics.FillEllipse(Brushes.Red, rootDotLeft, rootDotTop, rootDotDiameter, rootDotDiameter);
        graphics.DrawEllipse(Pens.DarkRed, rootDotLeft, rootDotTop, rootDotDiameter, rootDotDiameter);

        using (Font font = new Font("Arial", 10, FontStyle.Bold))
        {
          graphics.DrawString("x = " + rootX.ToString("F3"), font, Brushes.Black, rootPixelX + 10, rootPixelY - 20);
        }
      }

      using (Font font = new Font("Arial", 9, FontStyle.Italic))
      {
        graphics.DrawString("a = " + startPoint.ToString("0.####"), font, Brushes.Gray, axPixel + 2, imageHeight - 18);
        graphics.DrawString("b = " + endPoint.ToString("0.####"), font, Brushes.Gray, bxPixel + 2, imageHeight - 18);
      }

      pictureBox.Image = bitmap;

      graphics.Dispose();
      axisPen.Dispose();
      curvePen.Dispose();
      intervalPen.Dispose();
    }

    private void ComputeVisibleXRange(
        string formula,
        double a,
        double b,
        FunctionCalculator functionCalculator,
        out double graphStartX,
        out double graphEndX)
    {
      double innerYMin = double.MaxValue;
      double innerYMax = double.MinValue;
      int innerScan = 200;

      for (int i = 0; i <= innerScan; i++)
      {
        double x = a + (b - a) * i / innerScan;
        double y;
        try { y = functionCalculator.Calculate(formula, x); }
        catch { continue; }
        if (double.IsNaN(y) || double.IsInfinity(y)) continue;
        if (y < innerYMin) innerYMin = y;
        if (y > innerYMax) innerYMax = y;
      }

      if (innerYMin == double.MaxValue || innerYMax == double.MinValue)
      {
        innerYMin = -10;
        innerYMax = 10;
      }

      double ySpan = Math.Max(innerYMax - innerYMin, 1.0);
      double lowBound = innerYMin - ySpan * yToleranceFactor;
      double highBound = innerYMax + ySpan * yToleranceFactor;

      double xRange = b - a;
      double expand = xRange * xExpandFactor;

      graphStartX = a - expand;
      graphEndX = b + expand;

      double xStep = (expand * 2 + xRange) / trimScanSteps;

      while (graphStartX < a)
      {
        double y;
        bool isAcceptable;
        try
        {
          y = functionCalculator.Calculate(formula, graphStartX);
          isAcceptable = double.IsNaN(y) == false && double.IsInfinity(y) == false
                         && y >= lowBound && y <= highBound;
        }
        catch { isAcceptable = false; }

        if (isAcceptable) break;
        graphStartX += xStep;
      }
      if (graphStartX > a) graphStartX = a;

      while (graphEndX > b)
      {
        double y;
        bool isAcceptable;
        try
        {
          y = functionCalculator.Calculate(formula, graphEndX);
          isAcceptable = double.IsNaN(y) == false && double.IsInfinity(y) == false
                         && y >= lowBound && y <= highBound;
        }
        catch { isAcceptable = false; }

        if (isAcceptable) break;
        graphEndX -= xStep;
      }
      if (graphEndX < b) graphEndX = b;
    }

    private void FlushPoints(Graphics graphics, Pen pen, Point[] points, int count)
    {
      if (count <= 1) return;

      Point[] drawPoints = new Point[count];
      Array.Copy(points, drawPoints, count);
      graphics.DrawLines(pen, drawPoints);
    }

    private int SafeDoubleToInt(double value)
    {
      if (double.IsNaN(value) || double.IsInfinity(value)) return startIndex;
      if (value > maxPixelLimit) return maxPixelLimit;
      if (value < -maxPixelLimit) return -maxPixelLimit;
      return (int)value;
    }
  }
}