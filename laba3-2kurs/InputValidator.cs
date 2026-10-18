using System;
using System.Globalization;
using System.Windows.Forms;

namespace Laboratornaya1 {
  class InputValidator {
    private const double minAccuracyValue = 0;
    private const string commaSign = ",";
    private const string dotSign = ".";

    public bool TryReadDouble(TextBox textBox, string fieldName, out double value)
    {
      bool isParsed;
      string preparedText;

      // 1) Обрезаем пробелы
      // 2) Любую запятую превращаем в точку — так "0,001" и "0.001" становятся одним и тем же "0.001"
      preparedText = textBox.Text.Trim().Replace(commaSign, dotSign);

      // 3) NumberStyles.Float = знак + десятичная точка + экспонента.
      //    Без AllowThousands — "1.234" читается как 1.234, а не как 1234.
      //    InvariantCulture гарантирует, что "." — именно разделитель дробной части,
      //    независимо от того, что стоит в "Язык и региональные стандарты" Windows.
      isParsed = double.TryParse(
          preparedText,
          NumberStyles.Float,
          CultureInfo.InvariantCulture,
          out value
      );

      if (isParsed == false)
      {
        MessageBox.Show(
            "Некорректное значение " + fieldName + "." + Environment.NewLine +
            "Введите число. Дробная часть отделяется точкой или запятой." + Environment.NewLine +
            "Примеры: 0.001   или   0,001   или   1e-3",
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
        MessageBox.Show("Точность должна быть больше нуля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      return true;
    }

    public bool CheckInterval(double startPoint, double endPoint)
    {
      if (startPoint >= endPoint)
      {
        MessageBox.Show("Начало интервала должно быть меньше конца.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      return true;
    }

    public bool CheckFormula(string formula)
    {
      if (string.IsNullOrWhiteSpace(formula) == true)
      {
        MessageBox.Show("Формула не может быть пустой.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      return true;
    }
  }
}