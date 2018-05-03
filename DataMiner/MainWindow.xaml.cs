using DataMiner.Charts;
using DataMiner.ClusterDataReaders;
using DataMiner.Clustering;
using DataMiner.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataMiner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClusterData clusterData;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "CSV files (*.csv)|*.csv|TXT files (*.txt)|*.txt";

            if (dlg.ShowDialog() == true)
            {
                clusterData = new ClusterData();

                ClusterDataReader dr = ClusterDataReaderFactory.GetClusterDataReader(dlg.FileName, clusterData);

                if (dr == null)
                {
                    WPFMessageBox.MsgError("Hiba történt a file betöltése során!");
                }

                TextBlockFileName.Text = dlg.FileName;
                GridAllStatistics.DataContext = clusterData;
                GridXStatistics.DataContext = clusterData;
                GridYStatistics.DataContext = clusterData;

                clusterData.NormalizePoints();

                PointChart.Data = clusterData;
                PointChart.Draw();
            }
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadFile();
                TextBoxLog.Clear();
                DataGridClusterStats.ItemsSource = null;
            }
            catch (Exception ex)
            {
                WPFMessageBox.MsgError(ex.Message);
            }
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private Func<ClusterPoint, ClusterPoint, double> GetDistanceFunction()
        {
            Func<ClusterPoint, ClusterPoint, double> distanceFunction = Distance.Euclidean;

            switch (ComboBoxDistance.SelectedIndex)
            {
                case 0:
                    distanceFunction = Distance.Euclidean;
                    break;

                case 1:
                    distanceFunction = Distance.SquaredEuclidean;
                    break;

                case 2:
                    distanceFunction = Distance.Manhattan;
                    break;

                case 3:
                    distanceFunction = Distance.Chebyshev;
                    break;

                default:
                    distanceFunction = Distance.Euclidean;
                    break;
            }

            return distanceFunction;
        }

        private void ButtonClustering_Click(object sender, RoutedEventArgs e)
        {
            TextBoxLog.Clear();

            try
            {
                AlgorithmKMeans alg = new AlgorithmKMeans(clusterData, int.Parse(TextBoxNumberOfClusters.Text));
                alg.DistanceFunction = GetDistanceFunction();
                alg.MaxIterations = int.Parse(TextBoxMaximumIterations.Text);
                alg.Iteration += Alg_Iteration;

                alg.Run();

                PointChart.Data = clusterData;
                PointChart.Draw();

                DataGridClusterStats.ItemsSource = alg.GetClusterStatistics();
            }
            catch (NullReferenceException)
            {
                WPFMessageBox.MsgError("Empty dataset, please load a datafile.");
            }
            catch (Exception ex)
            {
                WPFMessageBox.MsgError("An error occured!", ex);
            }
        }

        private void Alg_Iteration(object sender, AlgorithmEventArgs e)
        {
            string log = $"Iteration: {e.IterationNumber}, Sum of errors: {e.SumOfErrors}, Sum of squared errors: {e.SumOfSquaredErrors}";
            TextBoxLog.AppendText(log + Environment.NewLine);
        }

        private void TextBoxNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;

                if (!string.IsNullOrEmpty(tb.Text))
                {
                    int a;

                    if (!int.TryParse(tb.Text, out a))
                    {
                        WPFMessageBox.MsgError("Number format error!");
                    }
                }
            }
        }
    }
}