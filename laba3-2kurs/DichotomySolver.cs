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
        out int iterationsCount,
        out string errorMessage)
    {
      double valueAtStart;
      double valueAtEnd;
      double valueAtMiddle;
      double middlePoint;
      bool isSolved;

      resultRoot = zeroValue;
      resultValue = zeroValue;
      iterationsCount = 0;
      errorMessage = "";
      isSolved = false;

      valueAtStart = functionCalculator.Calculate(formula, startPoint);
      valueAtEnd = functionCalculator.Calculate(formula, endPoint);

      if (valueAtStart * valueAtEnd > zeroValue)
      {
        errorMessage = "На заданном интервале нет корня (функция не меняет знак).";
        return false;
      }

      if (valueAtStart == zeroValue)
      {
        resultRoot = startPoint;
        resultValue = valueAtStart;
        isSolved = true;
        return isSolved;
      }

      if (valueAtEnd == zeroValue)
      {
        resultRoot = endPoint;
        resultValue = valueAtEnd;
        isSolved = true;
        return isSolved;
      }

      middlePoint = (startPoint + endPoint) / halfDivider;
      valueAtMiddle = functionCalculator.Calculate(formula, middlePoint);

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

      while ((endPoint - startPoint) > accuracy && iterationsCount < maxIterationsLimit)
      {
        middlePoint = (startPoint + endPoint) / halfDivider;
        valueAtMiddle = functionCalculator.Calculate(formula, middlePoint);

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

        ++iterationsCount;
      }

      resultRoot = (startPoint + endPoint) / halfDivider;
      resultValue = functionCalculator.Calculate(formula, resultRoot);
      isSolved = true;

      return isSolved;
    }
  }
}