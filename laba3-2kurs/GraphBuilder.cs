using System;
using System.Drawing;
using System.Windows.Forms;

namespace Laboratornaya1 {
  class GraphBuilder {
    private const double zeroValue = 0;
    private const double halfDivider = 2.0;
    private const double minScale = 1;
    private const int axisThickness = 1;
    private const int curveThickness = 2;
    private const int startIndex = 0;
    private const int rootDotRadius = 6;
    private const int rootDotBorder = 8;

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
      Bitmap bitmap;
      Graphics graphics;
      Pen axisPen;
      Pen curvePen;
      Brush backgroundBrush;
      Brush rootBrush;
      Pen rootPen;
      int width;
      int height;
      int centerX;
      int centerY;
      double step;
      double xValue;
      double yValue;
      double scaleX;
      double scaleY;
      double maxAbsY;
      int i;
      int pointsCount;
      Point[] points;
      double pixelX;
      double pixelY;
      int lastIndex;
      int rootPixelX;
      int rootPixelY;
      int rootDotLeft;
      int rootDotTop;

      width = pictureBox.Width;
      height = pictureBox.Height;

      if (width <= startIndex || height <= startIndex)
      {
        return;
      }

      bitmap = new Bitmap(width, height);
      graphics = Graphics.FromImage(bitmap);
      backgroundBrush = Brushes.White;
      graphics.FillRectangle(backgroundBrush, startIndex, startIndex, width, height);

      axisPen = new Pen(Color.Black, axisThickness);
      curvePen = new Pen(Color.Blue, curveThickness);
      rootBrush = Brushes.Red;
      rootPen = new Pen(Color.DarkRed, axisThickness);

      centerX = width / 2;
      centerY = height / 2;

      graphics.DrawLine(axisPen, startIndex, centerY, width, centerY);
      graphics.DrawLine(axisPen, centerX, startIndex, centerX, height);

      pointsCount = width;
      points = new Point[pointsCount];

      lastIndex = pointsCount - 1;
      step = (endPoint - startPoint) / lastIndex;
      maxAbsY = zeroValue;

      for (i = startIndex; i < pointsCount; ++i)
      {
        xValue = startPoint + i * step;
        yValue = functionCalculator.Calculate(formula, xValue);
        if (Math.Abs(yValue) > maxAbsY)
        {
          maxAbsY = Math.Abs(yValue);
        }
      }

      if (maxAbsY == zeroValue)
      {
        maxAbsY = minScale;
      }

      scaleX = width / (endPoint - startPoint);
      scaleY = (height / halfDivider) / maxAbsY;

      for (i = startIndex; i < pointsCount; ++i)
      {
        xValue = startPoint + i * step;
        yValue = functionCalculator.Calculate(formula, xValue);

        pixelX = (xValue - startPoint) * scaleX;
        pixelY = centerY - yValue * scaleY;

        points[i] = new Point((int)pixelX, (int)pixelY);
      }

      graphics.DrawLines(curvePen, points);

      if (showRoot == true)
      {
        pixelX = (rootX - startPoint) * scaleX;
        pixelY = centerY - rootY * scaleY;

        rootPixelX = (int)pixelX;
        rootPixelY = (int)pixelY;

        rootDotLeft = rootPixelX - rootDotRadius;
        rootDotTop = rootPixelY - rootDotRadius;

        graphics.FillEllipse(rootBrush, rootDotLeft, rootDotTop, rootDotBorder, rootDotBorder);
        graphics.DrawEllipse(rootPen, rootDotLeft, rootDotTop, rootDotBorder, rootDotBorder);
      }

      pictureBox.Image = bitmap;

      graphics.Dispose();
      axisPen.Dispose();
      curvePen.Dispose();
      rootPen.Dispose();
    }
  }
}