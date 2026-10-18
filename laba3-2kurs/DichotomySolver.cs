using System;

namespace Laboratornaya1 {
  class DichotomySolver {
    private const int maxIterationsLimit = 1000;
    private const double zeroValue = 0;
    private const double halfDivider = 2.0;

    public bool Solve(
        string formula,
        double startPoint,
        double endPoint,
        double accuracy,
        FunctionCalculator functionCalculator,
        out double resultRoot,
        out double resultValue,
        out string errorMessage)
    {
      double valueAtStart;
      double valueAtEnd;
      double valueAtMiddle;
      double middlePoint;
      int safetyCounter;

      resultRoot = zeroValue;
      resultValue = zeroValue;
      errorMessage = "";

      valueAtStart = functionCalculator.Calculate(formula, startPoint);
      valueAtEnd = functionCalculator.Calculate(formula, endPoint);

      if (double.IsNaN(valueAtStart) || double.IsInfinity(valueAtStart) ||
          double.IsNaN(valueAtEnd) || double.IsInfinity(valueAtEnd))
      {
        errorMessage = "Функция не определена или имеет разрыв на концах интервала.";
        return false;
      }

      if (valueAtStart * valueAtEnd > zeroValue)
      {
        errorMessage = "На заданном интервале нет корня (функция не меняет знак).";
        return false;
      }

      if (valueAtStart == zeroValue)
      {
        resultRoot = startPoint;
        resultValue = valueAtStart;
        return true;
      }

      if (valueAtEnd == zeroValue)
      {
        resultRoot = endPoint;
        resultValue = valueAtEnd;
        return true;
      }

      middlePoint = (startPoint + endPoint) / halfDivider;
      valueAtMiddle = functionCalculator.Calculate(formula, middlePoint);

      if (double.IsNaN(valueAtMiddle) || double.IsInfinity(valueAtMiddle))
      {
        errorMessage = "Внутри интервала есть разрыв (например, деление на ноль).";
        return false;
      }

      if (valueAtStart * valueAtMiddle < zeroValue)
      {
        endPoint = middlePoint;
        valueAtEnd = valueAtMiddle;
      }
      else
      {
        startPoint = middlePoint;
        valueAtStart = valueAtMiddle;
      }

      safetyCounter = 0;

      while ((endPoint - startPoint) > accuracy && safetyCounter < maxIterationsLimit)
      {
        middlePoint = (startPoint + endPoint) / halfDivider;
        valueAtMiddle = functionCalculator.Calculate(formula, middlePoint);

        if (double.IsNaN(valueAtMiddle) || double.IsInfinity(valueAtMiddle))
        {
          errorMessage = "Внутри интервала есть разрыв (например, деление на ноль).";
          return false;
        }

        if (valueAtStart * valueAtMiddle < zeroValue)
        {
          endPoint = middlePoint;
          valueAtEnd = valueAtMiddle;
        }
        else
        {
          startPoint = middlePoint;
          valueAtStart = valueAtMiddle;
        }

        ++safetyCounter;
      }

      resultRoot = (startPoint + endPoint) / halfDivider;
      resultValue = functionCalculator.Calculate(formula, resultRoot);
      return true;
    }
  }
}