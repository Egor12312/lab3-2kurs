using System;
using System.Data;
using System.Text.RegularExpressions;

namespace Laboratornaya1 {
  class FunctionCalculator {
    public double Calculate(string formula, double xValue)
    {
      string preparedFormula;
      DataTable dataTable;
      DataRow dataRow;
      double resultValue;
      string xString;

      preparedFormula = formula.ToLower();
      preparedFormula = preparedFormula.Replace("sin", "Sin");
      preparedFormula = preparedFormula.Replace("cos", "Cos");

      xString = xValue.ToString().Replace(",", ".");
      preparedFormula = preparedFormula.Replace("x", xString);

      preparedFormula = ConvertPowers(preparedFormula);

      dataTable = new DataTable();
      dataTable.Columns.Add("expression", typeof(string), preparedFormula);
      dataRow = dataTable.NewRow();
      dataTable.Rows.Add(dataRow);
      resultValue = double.Parse((string)dataRow["expression"]);

      return resultValue;
    }

    private string ConvertPowers(string expression)
    {
      string pattern;
      string result;
      Match match;
      string baseValue;
      string powerValue;
      int powerNumber;
      string replacement;
      int i;

      pattern = @"([a-z0-9\.]+)\^([0-9]+)";
      result = expression;

      while (true)
      {
        match = Regex.Match(result, pattern);

        if (match.Success == false)
        {
          break;
        }

        baseValue = match.Groups[1].Value;
        powerValue = match.Groups[2].Value;
        powerNumber = int.Parse(powerValue);
        replacement = baseValue;

        for (i = 1; i < powerNumber; ++i)
        {
          replacement = replacement + "*" + baseValue;
        }

        result = result.Substring(0, match.Index) + replacement + result.Substring(match.Index + match.Length);
      }

      return result;
    }
  }
}