using System;
using System.Data;
using System.Text.RegularExpressions;

namespace Laboratornaya1 {
  class FunctionCalculator {
    private const string powerPattern = @"([a-z0-9\.]+)\^([0-9]+)";
    private const string multiplicationSign = "*";
    private const string powerSign = "^";
    private const string dotSign = ".";
    private const string commaSign = ",";
    private const string sinusName = "sin";
    private const string cosinusName = "cos";
    private const string sinusReplacement = "Sin";
    private const string cosinusReplacement = "Cos";
    private const string xLetter = "x";
    private const string expressionColumnName = "expression";

    public double Calculate(string formula, double coordinateValue)
    {
      string preparedFormula;
      DataTable dataTable;
      DataRow dataRow;
      double resultValue;
      string coordinateString;

      preparedFormula = formula.ToLower();
      preparedFormula = preparedFormula.Replace(sinusName, sinusReplacement);
      preparedFormula = preparedFormula.Replace(cosinusName, cosinusReplacement);

      coordinateString = coordinateValue.ToString().Replace(commaSign, dotSign);
      preparedFormula = preparedFormula.Replace(xLetter, coordinateString);

      preparedFormula = ConvertPowers(preparedFormula);

      dataTable = new DataTable();
      dataTable.Columns.Add(expressionColumnName, typeof(string), preparedFormula);
      dataRow = dataTable.NewRow();
      dataTable.Rows.Add(dataRow);
      resultValue = double.Parse((string)dataRow[expressionColumnName]);

      return resultValue;
    }

    private string ConvertPowers(string expression)
    {
      string result;
      Match match;
      string baseValue;
      string powerValue;
      int powerNumber;
      string replacement;
      int multiplicationCounter;

      result = expression;

      while (true)
      {
        match = Regex.Match(result, powerPattern);

        if (match.Success == false)
        {
          break;
        }

        baseValue = match.Groups[1].Value;
        powerValue = match.Groups[2].Value;
        powerNumber = int.Parse(powerValue);
        replacement = baseValue;

        for (multiplicationCounter = 1; multiplicationCounter < powerNumber; ++multiplicationCounter)
        {
          replacement = replacement + multiplicationSign + baseValue;
        }

        result = result.Substring(0, match.Index) + replacement + result.Substring(match.Index + match.Length);
      }

      return result;
    }
  }
}