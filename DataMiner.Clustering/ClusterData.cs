using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace DataMiner.Clustering
{
    public class ClusterData
    {
        public List<ClusterPoint> Points { get; set; }
        public List<ClusterPoint> Centroids { get; set; }

        public double MinX => Points.Min(p => p.X);
        public double MaxX => Points.Max(p => p.X);
        public double RangeX => MaxX - MinX;
        public double AvgX => Points.Average(p => p.X);
        public double StdDevX => Sqrt(VarX);

        public double MinY => Points.Min(p => p.Y);
        public double MaxY => Points.Max(p => p.Y);
        public double RangeY => MaxY - MinY;
        public double AvgY => Points.Average(p => p.Y);
        public double StdDevY => Sqrt(VarY);

        public double VarX
        {
            get
            {
                double mean = AvgX;
                return Points.Sum(p => (p.X - mean) * (p.X - mean)) / Points.Count;
            }
        }

        public double VarY
        {
            get
            {
                double mean = AvgY;
                return Points.Sum(p => (p.Y - mean) * (p.Y - mean)) / Points.Count;
            }
        }

        public ClusterData()
        {
            Points = new List<ClusterPoint>();
            Centroids = new List<ClusterPoint>();
        }

        public ClusterData(List<ClusterPoint> points)
        {
            this.Points = points;
        }

        public void Add(ClusterPoint point)
        {
            Points.Add(point);
        }

        public void NormalizePoints()
        {
            double minX = MinX;
            double maxX = MaxX;
            double minY = MinY;
            double maxY = MaxY;

            foreach (ClusterPoint p in Points)
            {
                p.XT = (p.X - minX) / (maxX - minX);
                p.YT = (p.Y - minY) / (maxY - minY);
            }
        }

        public void NormalizeCentroids()
        {
            double minX = MinX;
            double maxX = MaxX;
            double minY = MinY;
            double maxY = MaxY;

            foreach (ClusterPoint p in Centroids)
            {
                p.XT = (p.X - minX) / (maxX - minX);
                p.YT = (p.Y - minY) / (maxY - minY);
            }
        }

        public List<int> GetClusterNumbers()
        {
            return Points.Select(p => p.ClusterNumber).Distinct().ToList();
        }

        public ClusterData GetClusterData(int number)
        {
            return new ClusterData(Points.Where(p => p.ClusterNumber == number).ToList());
        }

        public double AverageX(int number) => Points.Where(p => p.ClusterNumber == number).Average(p => p.X);
        public double AverageY(int number) => Points.Where(p => p.ClusterNumber == number).Average(p => p.Y);
    }
}