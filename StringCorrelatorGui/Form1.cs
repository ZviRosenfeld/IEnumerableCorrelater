using System;
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
                case "DamerauLevenshteinCorrelater":
                    correlator = new DamerauLevenshteinCorrelater<char>(10, 12, 7, 7);
                    break;
                case "LevenshteinCorrelater":
                default:
                    correlator = new LevenshteinCorrelater<char>(10, 7, 7);
                    break;
            }

            if (slowCompareCheclCheckBox.Checked)
                correlator = new SlowCorrelater<char>(correlator, 500);
            if (splitToChunksCheckBox.Checked)
                correlator = new SplitToChunksCorrelaterWrapper<char>(correlator, 10);

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
