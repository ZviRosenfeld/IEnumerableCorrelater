using System.Windows.Forms;
using System.Drawing;

namespace StringCorrelatorGui
{
    partial class StringCorrelatorUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.string1TextBox = new System.Windows.Forms.TextBox();
            this.string2TextBox = new System.Windows.Forms.TextBox();
            this.correlateButton = new System.Windows.Forms.Button();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.distancePanel = new System.Windows.Forms.Panel();
            this.distanceLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.string1ResultTextBox = new System.Windows.Forms.RichTextBox();
            this.string2ResultTextBox = new System.Windows.Forms.RichTextBox();
            this.controlPanel.SuspendLayout();
            this.distancePanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // string1TextBox
            // 
            this.string1TextBox.AutoSize = true;
            this.string1TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.string1TextBox.Location = new System.Drawing.Point(3, 3);
            this.string1TextBox.Multiline = false;
            this.string1TextBox.Name = "string1TextBox";
            this.string1TextBox.Size = new System.Drawing.Size(10000, 26);
            this.string1TextBox.TabIndex = 0;
            this.string1TextBox.Text = "";
            string1TextBox.Font = new Font(FontFamily.GenericMonospace, 16);
            // 
            // string2TextBox
            // 
            this.string2TextBox.AutoSize = true;
            this.string2TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.string2TextBox.Location = new System.Drawing.Point(3, 35);
            this.string2TextBox.Multiline = false;
            this.string2TextBox.Name = "string2TextBox";
            this.string2TextBox.Size = new System.Drawing.Size(10000, 26);
            this.string2TextBox.TabIndex = 1;
            this.string2TextBox.Text = "";
            string2TextBox.Font = new Font(FontFamily.GenericMonospace, 16);
            // 
            // correlateButton
            // 
            this.correlateButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.correlateButton.Location = new System.Drawing.Point(0, 0);
            this.correlateButton.Name = "correlateButton";
            this.correlateButton.Size = new System.Drawing.Size(160, 41);
            this.correlateButton.TabIndex = 2;
            this.correlateButton.Text = "Correlate";
            this.correlateButton.UseVisualStyleBackColor = true;
            this.correlateButton.Click += new System.EventHandler(this.correlateButton_Click);
            // 
            // controlPanel
            // 
            this.controlPanel.Controls.Add(this.distancePanel);
            this.controlPanel.Controls.Add(this.correlateButton);
            this.controlPanel.Location = new System.Drawing.Point(3, 67);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(10000, 41);
            this.controlPanel.TabIndex = 3;
            // 
            // distancePanel
            // 
            this.distancePanel.Controls.Add(this.distanceLabel);
            this.distancePanel.Controls.Add(this.label1);
            this.distancePanel.Location = new System.Drawing.Point(167, 4);
            this.distancePanel.Name = "distancePanel";
            this.distancePanel.Size = new System.Drawing.Size(148, 34);
            this.distancePanel.TabIndex = 3;
            // 
            // distanceLabel
            // 
            this.distanceLabel.AutoSize = true;
            this.distanceLabel.Location = new System.Drawing.Point(86, 6);
            this.distanceLabel.Name = "distanceLabel";
            this.distanceLabel.Size = new System.Drawing.Size(18, 20);
            this.distanceLabel.TabIndex = 1;
            this.distanceLabel.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Distance:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.string1TextBox);
            this.flowLayoutPanel1.Controls.Add(this.string2TextBox);
            this.flowLayoutPanel1.Controls.Add(this.controlPanel);
            this.flowLayoutPanel1.Controls.Add(this.string1ResultTextBox);
            this.flowLayoutPanel1.Controls.Add(this.string2ResultTextBox);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(849, 349);
            this.flowLayoutPanel1.TabIndex = 4;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // string1ResultTextBox
            // 
            this.string1ResultTextBox.Location = new System.Drawing.Point(3, 114);
            this.string1ResultTextBox.Name = "string1ResultTextBox";
            this.string1ResultTextBox.Size = new System.Drawing.Size(100, 96);
            this.string1ResultTextBox.TabIndex = 4;
            this.string1ResultTextBox.Text = "";
            string1ResultTextBox.Multiline = false;
            string1ResultTextBox.AutoSize = true;
            string1ResultTextBox.Dock = DockStyle.Fill;
            string1ResultTextBox.Enabled = false;
            string1ResultTextBox.Font = new Font(FontFamily.GenericMonospace, 16);
            // 
            // string2ResultTextBox
            // 
            this.string2ResultTextBox.Location = new System.Drawing.Point(3, 216);
            this.string2ResultTextBox.Name = "string2ResultTextBox";
            this.string2ResultTextBox.Size = new System.Drawing.Size(100, 96);
            this.string2ResultTextBox.TabIndex = 5;
            this.string2ResultTextBox.Text = "";
            string2ResultTextBox.Multiline = false;
            string2ResultTextBox.AutoSize = true;
            string2ResultTextBox.Dock = DockStyle.Fill;
            string2ResultTextBox.Enabled = false;
            string2ResultTextBox.Font = new Font(FontFamily.GenericMonospace, 16);
            // 
            // StringCorrelatorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "StringCorrelatorUserControl";
            this.Size = new System.Drawing.Size(849, 349);
            this.controlPanel.ResumeLayout(false);
            this.distancePanel.ResumeLayout(false);
            this.distancePanel.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TextBox string1TextBox;
        private TextBox string2TextBox;
        private Button correlateButton;
        private Panel controlPanel;
        private Panel distancePanel;
        private Label label1;
        private Label distanceLabel;
        private FlowLayoutPanel flowLayoutPanel1;
        private RichTextBox string1ResultTextBox;
        private RichTextBox string2ResultTextBox;
    }
}
