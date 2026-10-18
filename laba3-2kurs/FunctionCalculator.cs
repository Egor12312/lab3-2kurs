using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Laboratornaya1 {
  class FunctionCalculator {
    private const string coordinateFormat = "0.################";
    private const int maxPowerValue = 100;

    public double Calculate(string formula, double coordinateValue)
    {
      try
      {
        string preparedFormula = formula.ToLower();
        preparedFormula = preparedFormula.Replace(" ", "");
        preparedFormula = preparedFormula.Replace(",", ".");

        string coordinateString = coordinateValue.ToString(coordinateFormat, CultureInfo.InvariantCulture);
        if (coordinateString.StartsWith("-"))
        {
          // (0-2.5) вместо (-2.5) — чтобы не зависеть от поддержки унарного минуса в DataTable
          coordinateString = "(0" + coordinateString + ")";
        }

        // Заменяем именно переменную x, не трогая буквы в именах функций (exp и т.п.)
        preparedFormula = Regex.Replace(preparedFormula, @"\bx\b", coordinateString);

        // Сначала функции (чтобы корректно обработать sin(x^2) и sin(x)^2),
        // затем степени
        preparedFormula = EvaluateFunctions(preparedFormula);
        preparedFormula = ConvertPowers(preparedFormula);

        DataTable dataTable = new DataTable();
        dataTable.Locale = CultureInfo.InvariantCulture;   // <-- ключевое исправление

        object computeResult = dataTable.Compute(preparedFormula, "");
        if (computeResult == null || computeResult == DBNull.Value)
          return double.NaN;

        return Convert.ToDouble(computeResult, CultureInfo.InvariantCulture);
      }
      catch
      {
        return double.NaN;
      }
    }

    private string ConvertPowers(string expression)
    {
      string result = expression;
      string pattern = @"(\([^)]+\)|[a-z0-9\.]+)\^([0-9]+)";

      while (true)
      {
        Match match = Regex.Match(result, pattern);
        if (match.Success == false) break;

        string baseValue = match.Groups[1].Value;
        int powerNumber;

        if (int.TryParse(match.Groups[2].Value, out powerNumber) == false || powerNumber > maxPowerValue)
        {
          powerNumber = maxPowerValue;
        }

        string replacement;
        if (powerNumber == 0)
        {
          replacement = "1";
        }
        else
        {
          replacement = baseValue;
          for (int i = 1; i < powerNumber; i++)
          {
            replacement = replacement + "*" + baseValue;
          }
        }

        result = result.Substring(0, match.Index) + replacement + result.Substring(match.Index + match.Length);
      }

      return result;
    }

    private string EvaluateFunctions(string expression)
    {
      string[] functionNames = { "sin", "cos", "tg", "tan", "exp", "ln", "lg", "sqrt", "abs" };
      string result = expression;
      bool changed = true;

      while (changed)
      {
        changed = false;

        foreach (string name in functionNames)
        {
          int index = FindFunction(result, name);
          if (index < 0) continue;

          int openParen = index + name.Length;
          int closeParen = FindMatchingParen(result, openParen);
          if (closeParen < 0) throw new ArgumentException("Unbalanced parentheses");

          string argument = result.Substring(openParen + 1, closeParen - openParen - 1);
          double argValue = Calculate(argument, 0);
          if (double.IsNaN(argValue) || double.IsInfinity(argValue))
            throw new ArgumentException("Invalid function argument");

          double funcResult = ApplyFunction(name, argValue);
          if (double.IsNaN(funcResult) || double.IsInfinity(funcResult))
            throw new ArgumentException("Invalid function result");

          string replacement = funcResult.ToString(CultureInfo.InvariantCulture);
          if (replacement.StartsWith("-"))
            replacement = "(" + replacement + ")";

          result = result.Substring(0, index) + replacement + result.Substring(closeParen + 1);
          changed = true;
          break;
        }
      }

      return result;
    }

    private int FindFunction(string expression, string name)
    {
      int searchStart = 0;

      while (true)
      {
        int index = expression.IndexOf(name + "(", searchStart, StringComparison.Ordinal);
        if (index < 0) return -1;

        bool precededByLetter = index > 0 && char.IsLetter(expression[index - 1]);
        if (precededByLetter == false) return index;

        searchStart = index + 1;
      }
    }

    private int FindMatchingParen(string expression, int openIndex)
    {
      int depth = 0;

      for (int i = openIndex; i < expression.Length; i++)
      {
        if (expression[i] == '(')
        {
          depth++;
        }
        else if (expression[i] == ')')
        {
          depth--;
          if (depth == 0) return i;
        }
      }

      return -1;
    }

    private double ApplyFunction(string name, double argument)
    {
      switch (name)
      {
        case "sin": return Math.Sin(argument);
        case "cos": return Math.Cos(argument);
        case "tg":
        case "tan": return Math.Tan(argument);
        case "exp": return Math.Exp(argument);
        case "ln": return Math.Log(argument);
        case "lg": return Math.Log10(argument);
        case "sqrt": return Math.Sqrt(argument);
        case "abs": return Math.Abs(argument);
        default: throw new ArgumentException("Unknown function: " + name);
      }
    }
  }
}