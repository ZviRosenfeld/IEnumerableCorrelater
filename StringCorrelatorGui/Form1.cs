﻿using System;
using System.Windows.Forms;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.CorrelaterWrappers;
using IEnumerableCorrelater.Interfaces;

namespace StringCorrelatorGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SetCorrelator(object sender, EventArgs e) =>
            SetCorrelator();

        private void SetCorrelator()
        {
            ICorrelater<char> correlator;
            switch (selectCorrelatorComboBox.Text)
            {
                case nameof(DamerauLevenshteinCorrelater<char>):
                    correlator = new DamerauLevenshteinCorrelater<char>(10, 12, 7, 7);
                    break;
                case nameof(DynamicLcsCorrelater<char>):
                    correlator = new DynamicLcsCorrelater<char>();
                    break;
                case nameof(MyersAlgorithmCorrelater<char>):
                    correlator = new MyersAlgorithmCorrelater<char>();
                    break;
                case nameof(PatienceDiffCorrelater<char>):
                    correlator = new PatienceDiffCorrelater<char>();
                    break;
                case nameof(HuntSzymanskiCorrelater<char>):
                    correlator = new HuntSzymanskiCorrelater<char>();
                    break;
                case nameof(NullCorrelator<char>):
                    correlator = new NullCorrelator<char>();
                    break;
                case nameof(LevenshteinCorrelater<char>):
                default:
                    correlator = new LevenshteinCorrelater<char>(10, 7, 7);
                    break;
            }

            if (slowCompareCheclCheckBox.Checked)
                correlator = new SlowCorrelater<char>(correlator, 500);
            if (splitToChunksCheckBox.Checked)
                correlator = new SplitToChunksCorrelaterWrapper<char>(correlator, 10);
            if (IgnoreIdenticalBeginningAndEndCorrelaterWrapperCheckBox.Checked)
                correlator = new IgnoreIdenticalBeginningAndEndCorrelaterWrapper<char>(correlator);
            if (SplitByPatienceWrapperCheckBox.Checked)
                correlator = new SplitByPatienceAlgorithmWrapper<char>(correlator);

            stringCorrelatorUserControl1.Correlater = correlator;
        }

        private void generateRandomStringButton_Click(object sender, EventArgs e)
        {
            var s = Utils.GetLongString((int) randomStringLengthInput.Value);
            stringCorrelatorUserControl1.String1 = s;
            stringCorrelatorUserControl1.String2 = s;
        }
    }
}
