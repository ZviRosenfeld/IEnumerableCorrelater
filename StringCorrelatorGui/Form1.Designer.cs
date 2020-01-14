using System.Windows.Forms;

namespace StringCorrelatorGui
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.controlPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.selectCorrelatorPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.selectCorrelatorComboBox = new System.Windows.Forms.ComboBox();
            this.generateRandomStringPanel = new System.Windows.Forms.Panel();
            this.generateRandomStringButton = new System.Windows.Forms.Button();
            this.randomStringLengthInput = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.optimizationsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.splitToChunksCheckBox = new System.Windows.Forms.CheckBox();
            this.stringCorrelatorUserControl1 = new StringCorrelatorGui.StringCorrelatorUserControl();
            this.controlPanel.SuspendLayout();
            this.selectCorrelatorPanel.SuspendLayout();
            this.generateRandomStringPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.randomStringLengthInput)).BeginInit();
            this.optimizationsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlPanel
            // 
            this.controlPanel.Controls.Add(this.selectCorrelatorPanel);
            this.controlPanel.Controls.Add(this.generateRandomStringPanel);
            this.controlPanel.Controls.Add(this.optimizationsPanel);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Margin = new System.Windows.Forms.Padding(2);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(856, 88);
            this.controlPanel.TabIndex = 1;
            // 
            // selectCorrelatorPanel
            // 
            this.selectCorrelatorPanel.Controls.Add(this.label1);
            this.selectCorrelatorPanel.Controls.Add(this.selectCorrelatorComboBox);
            this.selectCorrelatorPanel.Location = new System.Drawing.Point(2, 2);
            this.selectCorrelatorPanel.Margin = new System.Windows.Forms.Padding(2);
            this.selectCorrelatorPanel.Name = "selectCorrelatorPanel";
            this.selectCorrelatorPanel.Size = new System.Drawing.Size(253, 32);
            this.selectCorrelatorPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Correlator";
            // 
            // selectCorrelatorComboBox
            // 
            this.selectCorrelatorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectCorrelatorComboBox.FormattingEnabled = true;
            this.selectCorrelatorComboBox.Items.AddRange(new object[] {
            "LevenshteinCorrelater",
            "DamerauLevenshteinCorrelater"});
            this.selectCorrelatorComboBox.Location = new System.Drawing.Point(58, 2);
            this.selectCorrelatorComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.selectCorrelatorComboBox.Name = "selectCorrelatorComboBox";
            this.selectCorrelatorComboBox.Size = new System.Drawing.Size(183, 21);
            this.selectCorrelatorComboBox.TabIndex = 1;
            this.selectCorrelatorComboBox.SelectedIndexChanged += new System.EventHandler(SetCorrelator);
            // 
            // generateRandomStringPanel
            // 
            this.generateRandomStringPanel.Controls.Add(this.generateRandomStringButton);
            this.generateRandomStringPanel.Controls.Add(this.randomStringLengthInput);
            this.generateRandomStringPanel.Controls.Add(this.label3);
            this.generateRandomStringPanel.Controls.Add(this.label2);
            this.generateRandomStringPanel.Location = new System.Drawing.Point(259, 2);
            this.generateRandomStringPanel.Margin = new System.Windows.Forms.Padding(2);
            this.generateRandomStringPanel.Name = "generateRandomStringPanel";
            this.generateRandomStringPanel.Size = new System.Drawing.Size(133, 76);
            this.generateRandomStringPanel.TabIndex = 1;
            // 
            // generateRandomStringButton
            // 
            this.generateRandomStringButton.Location = new System.Drawing.Point(5, 48);
            this.generateRandomStringButton.Margin = new System.Windows.Forms.Padding(2);
            this.generateRandomStringButton.Name = "generateRandomStringButton";
            this.generateRandomStringButton.Size = new System.Drawing.Size(123, 26);
            this.generateRandomStringButton.TabIndex = 3;
            this.generateRandomStringButton.Text = "Generate";
            this.generateRandomStringButton.UseVisualStyleBackColor = true;
            this.generateRandomStringButton.Click += new System.EventHandler(this.generateRandomStringButton_Click);
            // 
            // randomStringLengthInput
            // 
            this.randomStringLengthInput.Location = new System.Drawing.Point(74, 27);
            this.randomStringLengthInput.Margin = new System.Windows.Forms.Padding(2);
            this.randomStringLengthInput.Name = "randomStringLengthInput";
            this.randomStringLengthInput.Size = new System.Drawing.Size(39, 20);
            this.randomStringLengthInput.TabIndex = 2;
            this.randomStringLengthInput.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 29);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Length";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Generate Random String";
            // 
            // optimizationsPanel
            // 
            this.optimizationsPanel.Controls.Add(this.label4);
            this.optimizationsPanel.Controls.Add(this.splitToChunksCheckBox);
            this.optimizationsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.optimizationsPanel.Location = new System.Drawing.Point(397, 3);
            this.optimizationsPanel.Name = "optimizationsPanel";
            this.optimizationsPanel.Size = new System.Drawing.Size(136, 73);
            this.optimizationsPanel.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Wrappers";
            // 
            // splitToChunksCheckBox
            // 
            this.splitToChunksCheckBox.AutoSize = true;
            this.splitToChunksCheckBox.Location = new System.Drawing.Point(3, 16);
            this.splitToChunksCheckBox.Name = "splitToChunksCheckBox";
            this.splitToChunksCheckBox.Size = new System.Drawing.Size(97, 17);
            this.splitToChunksCheckBox.TabIndex = 0;
            this.splitToChunksCheckBox.Text = "Split to Chunks";
            this.splitToChunksCheckBox.UseVisualStyleBackColor = true;
            this.splitToChunksCheckBox.CheckedChanged += new System.EventHandler(SetCorrelator);
            // 
            // stringCorrelatorUserControl1
            // 
            this.stringCorrelatorUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stringCorrelatorUserControl1.Location = new System.Drawing.Point(0, 88);
            this.stringCorrelatorUserControl1.Margin = new System.Windows.Forms.Padding(1);
            this.stringCorrelatorUserControl1.Name = "stringCorrelatorUserControl1";
            this.stringCorrelatorUserControl1.Size = new System.Drawing.Size(856, 196);
            this.stringCorrelatorUserControl1.String1 = "";
            this.stringCorrelatorUserControl1.String2 = "";
            this.stringCorrelatorUserControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 284);
            this.Controls.Add(this.stringCorrelatorUserControl1);
            this.Controls.Add(this.controlPanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "StringCorrelater";
            this.controlPanel.ResumeLayout(false);
            this.selectCorrelatorPanel.ResumeLayout(false);
            this.selectCorrelatorPanel.PerformLayout();
            this.generateRandomStringPanel.ResumeLayout(false);
            this.generateRandomStringPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.randomStringLengthInput)).EndInit();
            this.optimizationsPanel.ResumeLayout(false);
            this.optimizationsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private StringCorrelatorUserControl stringCorrelatorUserControl1;
        private FlowLayoutPanel controlPanel;
        private FlowLayoutPanel selectCorrelatorPanel;
        private Label label1;
        private ComboBox selectCorrelatorComboBox;
        private Panel generateRandomStringPanel;
        private Label label2;
        private Label label3;
        private Button generateRandomStringButton;
        private NumericUpDown randomStringLengthInput;
        private FlowLayoutPanel optimizationsPanel;
        private CheckBox splitToChunksCheckBox;
        private Label label4;
    }
}

