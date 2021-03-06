﻿using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using IEnumerableCorrelater;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Interfaces;

namespace StringCorrelatorGui
{
    public partial class StringCorrelatorUserControl : UserControl
    {
        private ICorrelater<char> correlater;
        private long distnace = 0;
        private string string1;
        private string string2;

        public StringCorrelatorUserControl()
        {
            correlater = new LevenshteinCorrelater<char>(10, 7 , 7);
            InitializeComponent();
        }

        public ICorrelater<char> Correlater
        {
            set
            {
                correlater = value;
                if (value is IContinuousCorrelater<char> continuousCorrelater)
                    continuousCorrelater.OnResultUpdate += (partialResult) =>
                    {
                        distnace += partialResult.Distance;
                        string1 += new string(partialResult.BestMatch1);
                        string2 += new string(partialResult.BestMatch2);
                        var tempResult = new CorrelaterResult<char>(distnace, string1.ToCharArray(), string2.ToCharArray());
                        FillResultTextBoxs(tempResult);
                        Refresh();
                    };
                }
        }

        public string String1
        {
            get => string1TextBox.Text;
            set => string1TextBox.Text = value;
        }

        public string String2
        {
            get => string2TextBox.Text;
            set => string2TextBox.Text = value;
        }

        private void correlateButton_Click(object sender, System.EventArgs e)
        {
            correlateButton.Enabled = false;
            distnace = 0;
            string1 = string.Empty;
            string2 = string.Empty;

            var result = correlater.Correlate(string1TextBox.Text, string2TextBox.Text);
            if (!(correlater is IContinuousCorrelater<char>))
            {
                distanceLabel.Text = result.Distance.ToString();
                FillResultTextBoxs(result);
            }

            correlateButton.Enabled = true;
        }

        private void FillResultTextBoxs(CorrelaterResult<char> result)
        {
            string1ResultTextBox.Text = new string(result.BestMatch1.Select(GetCharPresentation).ToArray());
            string2ResultTextBox.Text = new string(result.BestMatch2.Select(GetCharPresentation).ToArray());

            ColorText(result);
        }

        private void ColorText(CorrelaterResult<char> result)
        {
            for (int i = 0; i < result.BestMatch1.Length; i++)
                if (result.BestMatch1[i] != result.BestMatch2[i])
                {
                    var color = GetColor(result.BestMatch1[i], result.BestMatch2[i]);
                    ChangeColor(string1ResultTextBox, i, color);
                    ChangeColor(string2ResultTextBox, i, color);
                }
        }


        private char GetCharPresentation(char c) =>
            c == '\0' ? ' ' : c;

        private Color GetColor(char c1, char c2) =>
            c1 == '\0' || c2 == '\0' ? Color.LightCoral : Color.Red;

        private void ChangeColor(RichTextBox textBox, int index, Color color)
        {
            textBox.SelectionStart = index;
            textBox.SelectionLength = 1;
            textBox.SelectionColor = color;
        }
    }
}
