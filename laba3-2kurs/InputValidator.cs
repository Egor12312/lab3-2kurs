using System;
using System.Windows.Forms;

namespace Laboratornaya1 {
  class InputValidator {
    private const double minAccuracyValue = 0;

    public bool TryReadDouble(TextBox textBox, string fieldName, out double value)
    {
      bool isParsed;

      isParsed = double.TryParse(textBox.Text, out value);

      if (isParsed == false)
      {
        MessageBox.Show(
            "Некорректное значение " + fieldName + ".",
            "Ошибка",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
      }

      return isParsed;
    }

    public bool CheckAccuracy(double accuracy)
    {
      if (accuracy <= minAccuracyValue)
      {
        MessageBox.Show(
            "Точность должна быть больше нуля.",
            "Ошибка",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
        return false;
      }

      return true;
    }

    public bool CheckInterval(double startPoint, double endPoint)
    {
      if (startPoint >= endPoint)
      {
        MessageBox.Show(
            "Начало интервала должно быть меньше конца.",
            "Ошибка",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
        return false;
      }

      return true;
    }

    public bool CheckFormula(string formula)
    {
      if (string.IsNullOrWhiteSpace(formula) == true)
      {
        MessageBox.Show(
            "Формула не может быть пустой.",
            "Ошибка",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
        return false;
      }

      return true;
    }
  }
}