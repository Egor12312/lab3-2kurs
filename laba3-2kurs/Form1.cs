using System;
using System.Windows.Forms;

namespace Laboratornaya1 {
  public partial class Form1 : Form {
    private const string fieldNameA = "a";
    private const string fieldNameB = "b";
    private const string fieldNameE = "e";
    private const string decimalFormatPrefix = "F";
    private const string emptyString = "";
    private const int minDecimalPlaces = 0;

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
      PerformCalculationAndPlot();
    }

    private void buttonPlot_Click(object sender, EventArgs e)
    {
      PerformCalculationAndPlot();
    }

    private void PerformCalculationAndPlot()
    {
      double startPoint;
      double endPoint;
      double accuracy;
      string formulaString;
      double resultRoot;
      double resultValue;
      string errorMessage;
      bool isSuccess;
      int decimalPlaces;
      double roundedRoot;
      double roundedValue;

      if (inputValidator.TryReadDouble(textBoxA, fieldNameA, out startPoint) == false) return;
      if (inputValidator.TryReadDouble(textBoxB, fieldNameB, out endPoint) == false) return;
      if (inputValidator.TryReadDouble(textBoxE, fieldNameE, out accuracy) == false) return;
      if (inputValidator.CheckAccuracy(accuracy) == false) return;
      if (inputValidator.CheckInterval(startPoint, endPoint) == false) return;

      formulaString = textBoxF.Text;
      if (inputValidator.CheckFormula(formulaString) == false) return;

      isSuccess = dichotomySolver.Solve(
          formulaString, startPoint, endPoint, accuracy, functionCalculator,
          out resultRoot, out resultValue, out errorMessage
      );

      if (isSuccess == true)
      {
        graphBuilder.BuildWithRoot(
            pictureBoxGraph, formulaString, startPoint, endPoint, functionCalculator, resultRoot, resultValue
        );

        decimalPlaces = GetDecimalPlaces(accuracy);
        roundedRoot = Math.Round(resultRoot, decimalPlaces);
        roundedValue = Math.Round(resultValue, decimalPlaces);

        labelResult.Text = "Найденный корень:" + Environment.NewLine +
                           "x = " + roundedRoot.ToString(decimalFormatPrefix + decimalPlaces) + Environment.NewLine +
                           "f(x) = " + roundedValue.ToString(decimalFormatPrefix + decimalPlaces);
      }
      else
      {
        // Даже если корень не найден — показываем график, чтобы видеть, что с функцией
        graphBuilder.Build(pictureBoxGraph, formulaString, startPoint, endPoint, functionCalculator);

        labelResult.Text = "Корень не найден." + Environment.NewLine + errorMessage;
        MessageBox.Show(errorMessage, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void menuItemClear_Click(object sender, EventArgs e)
    {
      textBoxA.Text = emptyString;
      textBoxB.Text = emptyString;
      textBoxE.Text = emptyString;
      textBoxF.Text = emptyString;
      labelResult.Text = emptyString;
      pictureBoxGraph.Image = null;
      pictureBoxGraph.Refresh();
    }

    private int GetDecimalPlaces(double accuracy)
    {
      int decimalPlaces;
      decimalPlaces = (int)Math.Round(-Math.Log10(accuracy));

      if (decimalPlaces < minDecimalPlaces)
      {
        decimalPlaces = minDecimalPlaces;
      }

      return decimalPlaces;
    }
  }
}