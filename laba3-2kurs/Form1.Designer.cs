namespace Laboratornaya1 {
  partial class Form1 {
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem menuItemCalculate;
    private System.Windows.Forms.ToolStripMenuItem menuItemClear;
    private System.Windows.Forms.Label labelA;
    private System.Windows.Forms.TextBox textBoxA;
    private System.Windows.Forms.Label labelB;
    private System.Windows.Forms.TextBox textBoxB;
    private System.Windows.Forms.Label labelE;
    private System.Windows.Forms.TextBox textBoxE;
    private System.Windows.Forms.Label labelF;
    private System.Windows.Forms.TextBox textBoxF;
    private System.Windows.Forms.Button buttonPlot;
    private System.Windows.Forms.PictureBox pictureBoxGraph;
    private System.Windows.Forms.Label labelResult;

    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.menuItemCalculate = new System.Windows.Forms.ToolStripMenuItem();
      this.menuItemClear = new System.Windows.Forms.ToolStripMenuItem();
      this.labelA = new System.Windows.Forms.Label();
      this.textBoxA = new System.Windows.Forms.TextBox();
      this.labelB = new System.Windows.Forms.Label();
      this.textBoxB = new System.Windows.Forms.TextBox();
      this.labelE = new System.Windows.Forms.Label();
      this.textBoxE = new System.Windows.Forms.TextBox();
      this.labelF = new System.Windows.Forms.Label();
      this.textBoxF = new System.Windows.Forms.TextBox();
      this.buttonPlot = new System.Windows.Forms.Button();
      this.pictureBoxGraph = new System.Windows.Forms.PictureBox();
      this.labelResult = new System.Windows.Forms.Label();
      this.menuStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGraph)).BeginInit();
      this.SuspendLayout();

      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuItemCalculate,
                this.menuItemClear});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(600, 24);
      this.menuStrip1.TabIndex = 0;
      this.menuStrip1.Text = "menuStrip1";

      this.menuItemCalculate.Name = "menuItemCalculate";
      this.menuItemCalculate.Text = "Рассчитать";
      this.menuItemCalculate.Click += new System.EventHandler(this.menuItemCalculate_Click);

      this.menuItemClear.Name = "menuItemClear";
      this.menuItemClear.Text = "Очистить";
      this.menuItemClear.Click += new System.EventHandler(this.menuItemClear_Click);

      this.labelA.AutoSize = true;
      this.labelA.Location = new System.Drawing.Point(20, 45);
      this.labelA.Name = "labelA";
      this.labelA.Size = new System.Drawing.Size(25, 13);
      this.labelA.Text = "a =";

      this.textBoxA.Location = new System.Drawing.Point(70, 42);
      this.textBoxA.Name = "textBoxA";
      this.textBoxA.Size = new System.Drawing.Size(100, 20);
      this.textBoxA.TabIndex = 1;

      this.labelB.AutoSize = true;
      this.labelB.Location = new System.Drawing.Point(20, 75);
      this.labelB.Name = "labelB";
      this.labelB.Size = new System.Drawing.Size(25, 13);
      this.labelB.Text = "b =";

      this.textBoxB.Location = new System.Drawing.Point(70, 72);
      this.textBoxB.Name = "textBoxB";
      this.textBoxB.Size = new System.Drawing.Size(100, 20);
      this.textBoxB.TabIndex = 2;

      this.labelE.AutoSize = true;
      this.labelE.Location = new System.Drawing.Point(20, 105);
      this.labelE.Name = "labelE";
      this.labelE.Size = new System.Drawing.Size(25, 13);
      this.labelE.Text = "e =";

      this.textBoxE.Location = new System.Drawing.Point(70, 102);
      this.textBoxE.Name = "textBoxE";
      this.textBoxE.Size = new System.Drawing.Size(100, 20);
      this.textBoxE.TabIndex = 3;

      this.labelF.AutoSize = true;
      this.labelF.Location = new System.Drawing.Point(20, 135);
      this.labelF.Name = "labelF";
      this.labelF.Size = new System.Drawing.Size(40, 13);
      this.labelF.Text = "f(x) =";

      this.textBoxF.Location = new System.Drawing.Point(70, 132);
      this.textBoxF.Name = "textBoxF";
      this.textBoxF.Size = new System.Drawing.Size(200, 20);
      this.textBoxF.TabIndex = 4;

      this.buttonPlot.Location = new System.Drawing.Point(20, 165);
      this.buttonPlot.Name = "buttonPlot";
      this.buttonPlot.Size = new System.Drawing.Size(250, 30);
      this.buttonPlot.TabIndex = 5;
      this.buttonPlot.Text = "Построить график";
      this.buttonPlot.UseVisualStyleBackColor = true;
      this.buttonPlot.Click += new System.EventHandler(this.buttonPlot_Click);

      this.pictureBoxGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pictureBoxGraph.Location = new System.Drawing.Point(20, 205);
      this.pictureBoxGraph.Name = "pictureBoxGraph";
      this.pictureBoxGraph.Size = new System.Drawing.Size(550, 300);
      this.pictureBoxGraph.TabIndex = 6;
      this.pictureBoxGraph.TabStop = false;

      this.labelResult.AutoSize = true;
      this.labelResult.Location = new System.Drawing.Point(300, 45);
      this.labelResult.Name = "labelResult";
      this.labelResult.Size = new System.Drawing.Size(0, 13);
      this.labelResult.TabIndex = 7;

      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(600, 530);
      this.Controls.Add(this.labelResult);
      this.Controls.Add(this.pictureBoxGraph);
      this.Controls.Add(this.buttonPlot);
      this.Controls.Add(this.textBoxF);
      this.Controls.Add(this.labelF);
      this.Controls.Add(this.textBoxE);
      this.Controls.Add(this.labelE);
      this.Controls.Add(this.textBoxB);
      this.Controls.Add(this.labelB);
      this.Controls.Add(this.textBoxA);
      this.Controls.Add(this.labelA);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "Form1";
      this.Text = "Лабораторная работа №1. Метод дихотомии";
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGraph)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}