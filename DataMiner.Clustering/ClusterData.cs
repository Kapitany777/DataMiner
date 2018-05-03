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

        public int Count => Points.Count;

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
        
        public int ClusterCount(int number) => Points.Where(p => p.ClusterNumber == number).Count();

        public double ClusterMinX(int number) => Points.Where(p => p.ClusterNumber == number).Min(p => p.X);
        public double ClusterMaxX(int number) => Points.Where(p => p.ClusterNumber == number).Max(p => p.X);
        public double ClusterAvgX(int number) => Points.Where(p => p.ClusterNumber == number).Average(p => p.X);
        public double ClusterStdDevX(int number) => Sqrt(ClusterVarX(number));

        public double ClusterMinY(int number) => Points.Where(p => p.ClusterNumber == number).Min(p => p.Y);
        public double ClusterMaxY(int number) => Points.Where(p => p.ClusterNumber == number).Max(p => p.Y);
        public double ClusterAvgY(int number) => Points.Where(p => p.ClusterNumber == number).Average(p => p.Y);
        public double ClusterStdDevY(int number) => Sqrt(ClusterVarY(number));

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

        public double ClusterVarX(int number)
        {
            if (ClusterCount(number) == 0)
            {
                return 0;
            }

            double mean = ClusterAvgX(number);

            return Points.Where(p => p.ClusterNumber == number).Sum(p => (p.X - mean) * (p.X - mean)) / ClusterCount(number);
        }

        public double ClusterVarY(int number)
        {
            if (ClusterCount(number) == 0)
            {
                return 0;
            }

            double mean = ClusterAvgY(number);

            return Points.Where(p => p.ClusterNumber == number).Sum(p => (p.Y - mean) * (p.Y - mean)) / ClusterCount(number);
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

        public void Reset()
        {
            foreach (ClusterPoint point in Points)
            {
                point.ClusterNumber = 0;
            }
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

        public List<ClusterStatistics> GetClusterStatistics()
        {
            List<ClusterStatistics> stat = new List<ClusterStatistics>();

            for (int i = 0; i < Centroids.Count; i++)
            {
                stat.Add(new ClusterStatistics
                {
                    ClusterNumber = i,
                    ClusterCount = ClusterCount(i),
                    MinX = ClusterMinX(i),
                    MaxX = ClusterMaxX(i),
                    AverageX = ClusterAvgX(i),
                    StdDevX = ClusterStdDevX(i),
                    MinY = ClusterMinY(i),
                    MaxY = ClusterMaxY(i),
                    AverageY = ClusterAvgY(i),
                    StdDevY = ClusterStdDevY(i),
                });
            }

            return stat;
        }
    }
}