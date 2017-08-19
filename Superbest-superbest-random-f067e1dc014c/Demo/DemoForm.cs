using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Superbest_random;

namespace Demo
{
    public partial class DemoForm : Form
    {
        public DemoForm()
        {
            InitializeComponent();
            demo_picker.SelectedItem = demo_picker.Items[4];
        }

        private void DemoRequested(object sender, EventArgs e)
        {
            demo_picker.Enabled = false;
            btn_execute.Enabled = false;

            var i = demo_picker.SelectedItem;
            var s = (string)i;

            var bw = new BackgroundWorker
                         {
                             WorkerReportsProgress = true
                         };

            if (s == "Random permutations - performance") PermutationPerformanceRequested(bw);
            else if (s == "Gaussian - distribution") GaussianDistributionDemo();
            else if (s == "Triangular - distribution") TriangularDistributionDemo();
            else if (s == "Boolean - distribution") BooleanDistributionDemo();
            else if (s.Contains("(not yet implemented)"))
            {
                MessageBox.Show("Look, it says not implemented. What are you, daft?");
                demo_picker.Enabled = true;
                btn_execute.Enabled = true;
            }
            else
            {
                MessageBox.Show("Please select a demo from the drop down.");
                demo_picker.Enabled = true;
                btn_execute.Enabled = true;
            }
        }

        private void GaussianDistributionDemo()
        {
            var sigma = 1d;
            var mu = 0d;
            var n_bins = 51;

            var bins = new int[n_bins];
            var r = new Random();
            for (var i = 0; i < n_bins * 10000; i++)
            {
                var g = r.NextGaussian(mu, sigma);

                // Below code is for generating the histogram on the fly
                var z = (g - mu) / sigma;

                if (z > 3 || z < -3) continue;

                var b = (int)((z + 3) * n_bins / 6d);
                bins[b]++;
            }

            var f = new Series("Histogram");
            graph.Series.Clear();
            graph.Series.Add(f);
            graph.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            graph.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            graph.ChartAreas[0].AxisX.Minimum = mu - 3 * sigma;
            graph.ChartAreas[0].AxisX.Title = "Value";
            graph.ChartAreas[0].AxisY.Title = "Count";

            graph.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            //graph.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.##}";
            graph.Legends.Clear();

            for (var i = 0; i < n_bins; i++)
            {
                var x = (i * 2d / n_bins - 1) * 3 * sigma + mu;
                var y = bins[i];
                f.Points.AddXY(x, y);
            }

            demo_picker.Enabled = true;
            btn_execute.Enabled = true;
        }

        private void TriangularDistributionDemo()
        {
            var min = 2;
            var max = 5;
            var mode = 4;
            var n_bins = 51;

            var bins = new int[n_bins];
            var r = new Random();
            for (var i = 0; i < n_bins * 10000; i++)
            {
                var x = r.NextTriangular(min, max, mode);

                // Below code is for generating the histogram on the fly
                var z = (x - min) / (max - min);

                var b = (int)(z * n_bins);
                bins[b]++;
            }

            var f = new Series("Histogram");
            graph.Series.Clear();
            graph.Series.Add(f);
            graph.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            graph.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            //graph.ChartAreas[0].AxisX.Minimum = min;
            graph.ChartAreas[0].AxisX.Title = "Value";
            graph.ChartAreas[0].AxisY.Title = "Count";

            //graph.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            //graph.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.##}";
            graph.Legends.Clear();

            for (var i = 0; i < n_bins; i++)
            {
                var x = i * (max - min) + min;
                var y = bins[i];
                f.Points.AddXY(x, y);
            }

            demo_picker.Enabled = true;
            btn_execute.Enabled = true;
        }

        private void BooleanDistributionDemo()
        {
            var samples = 1000;
            var step = 10;

            var hit_rate = new double[samples];
            var r = new Random();
            for (var i = 0; i < samples; i++)
            {
                var hits = 0d;
                var n = (i + 1) * step;
                for (var j = 0; j < n; j++)
                {
                    var x = r.NextBoolean();
                    if (x) hits++;
                }

                hit_rate[i] = hits / ((i + 1) * step);
            }

            var f = new Series("Hit rate") { ChartType = SeriesChartType.Line };
            graph.Series.Clear();
            graph.Series.Add(f);
            graph.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            graph.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            graph.ChartAreas[0].AxisX.Minimum = 0;
            graph.ChartAreas[0].AxisY.Minimum = 0;
            graph.ChartAreas[0].AxisY.Maximum = 1;

            graph.ChartAreas[0].AxisX.Title = "Booleans drawn";
            graph.ChartAreas[0].AxisY.Title = "Fraction of Trues";

            //graph.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            //graph.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.##}";
            graph.Legends.Clear();

            for (var i = 0; i < samples; i++)
            {
                var x = i * step + 1;
                var y = hit_rate[i];
                f.Points.AddXY(x, y);
            }

            demo_picker.Enabled = true;
            btn_execute.Enabled = true;
        }
        private void PermutationPerformanceRequested(BackgroundWorker bw)
        {
            var f = new Series("Performance") { ChartType = SeriesChartType.Point };

            bw.DoWork += PermutationPerformanceDemo;
            bw.ProgressChanged += (o, args) =>
                {
                    var data = (double[])args.UserState;
                    f.Points.AddXY(data[0], data[1]);
                };
            bw.RunWorkerCompleted += (o, args) =>
                {
                    var fit = new Series("Quadratic fit") { ChartType = SeriesChartType.Line };
                    graph.Series.Add(fit);

                    graph.DataManipulator.FinancialFormula(
                        FinancialFormula.Forecasting,
                        "3,0,false,false",
                        "Performance:Y",
                        "Quadratic fit:Y"
                        );

                    demo_picker.Enabled = true;
                    btn_execute.Enabled = true;
                };

            graph.Series.Clear();
            graph.Series.Add(f);
            
            graph.ChartAreas.Clear();
            graph.ChartAreas.Add(new ChartArea());
            var area = graph.ChartAreas[0];
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.Enabled = false;
            area.AxisX.Title = "k of Permutations(n, k)";
            area.AxisY.Title = "milliseconds";

            bw.RunWorkerAsync();
        }

        private static void PermutationPerformanceDemo(object sender, DoWorkEventArgs e)
        {
            var r = new Random();

            for (var k = 100; k <= 1000; k += 50)
            {
                var t = new List<double>();
                var s = new Stopwatch();

                for (var i = 0; i < 100; i++)
                {
                    s.Restart();
                    var p = r.Permutation(k * 100, k);
                    s.Stop();
                    t.Add(s.ElapsedMilliseconds);
                }

                var time = t.Average();
                ((BackgroundWorker)sender).ReportProgress(0, new[] { k, time });
            }
        }
    }
}
