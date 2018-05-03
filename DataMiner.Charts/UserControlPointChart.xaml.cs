using DataMiner.Clustering;
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

namespace DataMiner.Charts
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControlPointChart : UserControl
    {
        public ClusterData Data { get; set; }

        private List<Color> colors = new List<Color>
        {
            Colors.Black,
            Colors.Blue,
            Colors.Green,
            Colors.Cyan,
            Colors.Red,
            Colors.Magenta,
            Colors.Brown,
            Colors.White,
            Colors.Gray,
            Colors.LightBlue,
            Colors.LightGreen,
            Colors.LightCyan,
            Colors.Maroon,
            Colors.LightPink,
            Colors.Yellow
        };

        public UserControlPointChart()
        {
            InitializeComponent();

            AddColors();
        }
        
        private void AddColors()
        {
            for (int i = 0; i < 85; i++)
            {
                byte r = (byte)((i % 4) * 60);
                byte g = (byte)(i * 2);
                byte b = (byte)((i % 2) * 100);
                colors.Add(Color.FromRgb(r, g, b));
            }
        }

        public void Draw()
        {
            Clear();

            double width = CanvasChart.ActualWidth;
            double height = CanvasChart.ActualHeight;

            // Draw points
            foreach (ClusterPoint point in Data.Points)
            {
                Ellipse e = new Ellipse();
                e.Width = 10;
                e.Height = 10;
                e.Fill = new SolidColorBrush(colors[point.ClusterNumber]);

                e.SetValue(Canvas.LeftProperty, point.XT * width);
                e.SetValue(Canvas.TopProperty, height - point.YT * height);

                CanvasChart.Children.Add(e);
            }

            // Draw centroids
            foreach (ClusterPoint centroid in Data.Centroids)
            {
                Rectangle r = new Rectangle();
                r.Width = 15;
                r.Height = 15;
                r.Fill = new SolidColorBrush(Colors.LightBlue);
                r.Stroke = new SolidColorBrush(Colors.Blue);

                r.SetValue(Canvas.LeftProperty, centroid.XT * width);
                r.SetValue(Canvas.TopProperty, height - centroid.YT * height);

                CanvasChart.Children.Add(r);
            }
        }

        public void Clear()
        {
            CanvasChart.Children.Clear();
        }
    }
}
