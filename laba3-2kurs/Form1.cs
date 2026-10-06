using System;
using System.Windows.Forms;

namespace Laboratornaya1 {
  public partial class Form1 : Form {
    private InputValidator inputValidator;
    private FunctionCalculator functionCalculator;
    private DichotomySolver dichotomySolver;
    private GraphBuilder graphBuilder;

    public Form1()
    {
      InitializeComponent();

      inputValidator = new InputValidator();
      functionCalculator = new FunctionCalculator();
      dichotomySolver = new DichotomySolver();
      graphBuilder = new GraphBuilder();
    }

    private void menuItemCalculate_Click(object sender, EventArgs e)
    {
      double startPoint;
      double endPoint;
      double accuracy;
      string formulaString;
      double resultRoot;
      double resultValue;
      int iterationsCount;
      string errorMessage;
      bool isSuccess;
      int decimalPlaces;
      double roundedRoot;
      double roundedValue;

      if (inputValidator.TryReadDouble(textBoxA, "a", out startPoint) == false)
      {
        return;
      }

      if (inputValidator.TryReadDouble(textBoxB, "b", out endPoint) == false)
      {
        return;
      }

      if (inputValidator.TryReadDouble(textBoxE, "e", out accuracy) == false)
      {
        return;
      }

      if (inputValidator.CheckAccuracy(accuracy) == false)
      {
        return;
      }

      if (inputValidator.CheckInterval(startPoint, endPoint) == false)
      {
        return;
      }

      formulaString = textBoxF.Text;

      if (inputValidator.CheckFormula(formulaString) == false)
      {
        return;
      }

      isSuccess = dichotomySolver.Solve(
          formulaString,
          startPoint,
          endPoint,
          accuracy,
          functionCalculator,
          out resultRoot,
          out resultValue,
          out iterationsCount,
          out errorMessage
      );

      if (isSuccess == false)
      {
        labelResult.Text = "Корень не найден." + Environment.NewLine + errorMessage;
        MessageBox.Show(errorMessage, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      decimalPlaces = GetDecimalPlaces(accuracy);
      roundedRoot = Math.Round(resultRoot, decimalPlaces);
      roundedValue = Math.Round(resultValue, decimalPlaces);

      labelResult.Text = "Найденный корень:" + Environment.NewLine +
                         "x = " + roundedRoot.ToString("F" + decimalPlaces) + Environment.NewLine +
                         "f(x) = " + roundedValue.ToString("F" + decimalPlaces) + Environment.NewLine +
                         "Итераций: " + iterationsCount;
    }

    private void menuItemClear_Click(object sender, EventArgs e)
    {
      textBoxA.Text = "";
      textBoxB.Text = "";
      textBoxE.Text = "";
      textBoxF.Text = "";
      labelResult.Text = "";
      pictureBoxGraph.Image = null;
      pictureBoxGraph.Refresh();
    }

    private void buttonPlot_Click(object sender, EventArgs e)
    {
      double startPoint;
      double endPoint;
      double accuracy;
      string formulaString;
      double resultRoot;
      double resultValue;
      int iterationsCount;
      string errorMessage;
      bool isSuccess;
      int decimalPlaces;
      double roundedRoot;
      double roundedValue;

      if (inputValidator.TryReadDouble(textBoxA, "a", out startPoint) == false)
      {
        return;
      }

      if (inputValidator.TryReadDouble(textBoxB, "b", out endPoint) == false)
      {
        return;
      }

      if (inputValidator.TryReadDouble(textBoxE, "e", out accuracy) == false)
      {
        return;
      }

      if (inputValidator.CheckAccuracy(accuracy) == false)
      {
        return;
      }

      if (inputValidator.CheckInterval(startPoint, endPoint) == false)
      {
        return;
      }

      formulaString = textBoxF.Text;

      if (inputValidator.CheckFormula(formulaString) == false)
      {
        return;
      }

      isSuccess = dichotomySolver.Solve(
          formulaString,
          startPoint,
          endPoint,
          accuracy,
          functionCalculator,
          out resultRoot,
          out resultValue,
          out iterationsCount,
          out errorMessage
      );

      if (isSuccess == true)
      {
        graphBuilder.BuildWithRoot(
            pictureBoxGraph,
            formulaString,
            startPoint,
            endPoint,
            functionCalculator,
            resultRoot,
            resultValue
        );

        decimalPlaces = GetDecimalPlaces(accuracy);
        roundedRoot = Math.Round(resultRoot, decimalPlaces);
        roundedValue = Math.Round(resultValue, decimalPlaces);

        labelResult.Text = "Найденный корень:" + Environment.NewLine +
                           "x = " + roundedRoot.ToString("F" + decimalPlaces) + Environment.NewLine +
                           "f(x) = " + roundedValue.ToString("F" + decimalPlaces) + Environment.NewLine +
                           "Итераций: " + iterationsCount;
      }
      else
      {
        graphBuilder.Build(
            pictureBoxGraph,
            formulaString,
            startPoint,
            endPoint,
            functionCalculator
        );

        labelResult.Text = "Корень не найден." + Environment.NewLine + errorMessage;
        MessageBox.Show(errorMessage, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private int GetDecimalPlaces(double accuracy)
    {
      int decimalPlaces;

      decimalPlaces = (int)Math.Round(-Math.Log10(accuracy));

      if (decimalPlaces < 0)
      {
        decimalPlaces = 0;
      }

      return decimalPlaces;
    }
  }
}