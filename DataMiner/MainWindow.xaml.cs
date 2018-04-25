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
            dlg.Filter = "CSV files (*.csv)|*.csv";

            if (dlg.ShowDialog() == true)
            {
                clusterData = new ClusterData();

                ClusterDataReader dr = ClusterDataReaderFactory.GetClusterDataReader(dlg.FileName, clusterData);

                if (dr == null)
                {
                    WPFMessageBox.MsgError("Hiba történt a file betöltése során!");
                }

                GridXStatistics.DataContext = clusterData;
                GridYStatistics.DataContext = clusterData;
            }
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadFile();
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
    }
}