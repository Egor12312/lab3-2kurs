using System;
using System.Drawing;
using System.Windows.Forms;

namespace Laboratornaya1 {
  class GraphBuilder {
    private const double zeroValue = 0;
    private const double halfDivider = 2.0;
    private const double minScaleValue = 1;
    private const int axisThickness = 1;
    private const int curveThickness = 2;
    private const int startIndex = 0;
    private const int rootDotRadius = 6;
    private const int rootDotDiameter = 8;
    private const int pointsPadding = 1;
    private const int dividerForCenter = 2;

    public void Build(
        PictureBox pictureBox,
        string formula,
        double startPoint,
        double endPoint,
        FunctionCalculator functionCalculator)
    {
      BuildInternal(
          pictureBox,
          formula,
          startPoint,
          endPoint,
          functionCalculator,
          false,
          zeroValue,
          zeroValue);
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
      BuildInternal(
          pictureBox,
          formula,
          startPoint,
          endPoint,
          functionCalculator,
          true,
          rootX,
          rootY);
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
      int imageWidth;
      int imageHeight;
      int centerX;
      int centerY;
      double stepValue;
      double currentX;
      double currentY;
      double scaleX;
      double scaleY;
      double maxAbsY;
      int pointIndex;
      int pointsCount;
      Point[] points;
      double pixelX;
      double pixelY;
      int lastIndex;
      int rootPixelX;
      int rootPixelY;
      int rootDotLeft;
      int rootDotTop;

      imageWidth = pictureBox.Width;
      imageHeight = pictureBox.Height;

      if (imageWidth <= startIndex || imageHeight <= startIndex)
      {
        return;
      }

      bitmap = new Bitmap(imageWidth, imageHeight);
      graphics = Graphics.FromImage(bitmap);
      backgroundBrush = Brushes.White;
      graphics.FillRectangle(backgroundBrush, startIndex, startIndex, imageWidth, imageHeight);

      axisPen = new Pen(Color.Black, axisThickness);
      curvePen = new Pen(Color.Blue, curveThickness);
      rootBrush = Brushes.Red;
      rootPen = new Pen(Color.DarkRed, axisThickness);

      centerX = imageWidth / dividerForCenter;
      centerY = imageHeight / dividerForCenter;

      graphics.DrawLine(axisPen, startIndex, centerY, imageWidth, centerY);
      graphics.DrawLine(axisPen, centerX, startIndex, centerX, imageHeight);

      pointsCount = imageWidth;
      points = new Point[pointsCount];

      lastIndex = pointsCount - pointsPadding;
      stepValue = (endPoint - startPoint) / lastIndex;
      maxAbsY = zeroValue;

      for (pointIndex = startIndex; pointIndex < pointsCount; ++pointIndex)
      {
        currentX = startPoint + pointIndex * stepValue;
        currentY = functionCalculator.Calculate(formula, currentX);

        if (Math.Abs(currentY) > maxAbsY)
        {
          maxAbsY = Math.Abs(currentY);
        }
      }

      if (maxAbsY == zeroValue)
      {
        maxAbsY = minScaleValue;
      }

      scaleX = imageWidth / (endPoint - startPoint);
      scaleY = (imageHeight / halfDivider) / maxAbsY;

      for (pointIndex = startIndex; pointIndex < pointsCount; ++pointIndex)
      {
        currentX = startPoint + pointIndex * stepValue;
        currentY = functionCalculator.Calculate(formula, currentX);

        pixelX = (currentX - startPoint) * scaleX;
        pixelY = centerY - currentY * scaleY;

        points[pointIndex] = new Point((int)pixelX, (int)pixelY);
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

        graphics.FillEllipse(rootBrush, rootDotLeft, rootDotTop, rootDotDiameter, rootDotDiameter);
        graphics.DrawEllipse(rootPen, rootDotLeft, rootDotTop, rootDotDiameter, rootDotDiameter);
      }

      pictureBox.Image = bitmap;

      graphics.Dispose();
      axisPen.Dispose();
      curvePen.Dispose();
      rootPen.Dispose();
    }
  }
}