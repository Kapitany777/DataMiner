using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner.Clustering
{
    public class AlgorithmKMeans
    {
        public int K { get; private set; }
        public int MaxIterations { get; set; }
        public Func<ClusterPoint, ClusterPoint, double> DistanceFunction { get; set; }

        public event AlgorithmEventHandler Iteration;

        private ClusterData clusterData;
        
        public AlgorithmKMeans(ClusterData clusterData, int k)
        {
            this.clusterData = clusterData;
            this.K = k;

            this.MaxIterations = 20;
            this.DistanceFunction = Distance.Euclidean;
        }

        private void InitializeCentroids()
        {
            Random rnd = new Random();

            clusterData.Centroids = new List<ClusterPoint>();

            for (int i = 0; i < this.K; i++)
            {
                ClusterPoint p = clusterData.Points[rnd.Next(clusterData.Points.Count)];
                clusterData.Centroids.Add(new ClusterPoint(p, i));
            }
        }

        public void Run()
        {
            clusterData.Reset();
            InitializeCentroids();

            double sumOfSquaredErrors1 = this.SumOfSquaredErrors();

            for (int i = 0; i < this.MaxIterations; i++)
            {
                // Calculate cluster numbers
                foreach (ClusterPoint point in clusterData.Points)
                {
                    int minCluster = 0;
                    double minDistance = DistanceFunction(point, clusterData.Centroids[0]);

                    for (int j = 1; j < this.K; j++)
                    {
                        double d = DistanceFunction(point, clusterData.Centroids[j]);

                        if (d < minDistance)
                        {
                            minCluster = j;
                            minDistance = d;
                        }
                    }

                    point.ClusterNumber = minCluster;
                }

                // Recalculate centroids
                for (int j = 0; j < this.K; j++)
                {
                    clusterData.Centroids[j].X = clusterData.AverageX(j);
                    clusterData.Centroids[j].Y = clusterData.AverageY(j);
                }

                clusterData.NormalizeCentroids();

                double sumOfErrors = this.SumOfErrors();
                double sumOfSquaredErrors2 = this.SumOfSquaredErrors();
                Iteration?.Invoke(this, new AlgorithmEventArgs(i, sumOfErrors, sumOfSquaredErrors2));

                if (sumOfSquaredErrors1 == sumOfSquaredErrors2)
                {
                    break;
                }
                else
                {
                    sumOfSquaredErrors1 = sumOfSquaredErrors2;
                }
            }
        }

        public double SumOfErrors()
        {
            double sum = 0;

            foreach (ClusterPoint point in clusterData.Points)
            {
                sum += DistanceFunction(point, clusterData.Centroids[point.ClusterNumber]);
            }

            return sum;
        }

        public double SumOfSquaredErrors()
        {
            double sum = 0;

            foreach (ClusterPoint point in clusterData.Points)
            {
                sum += Distance.SquaredEuclidean(point, clusterData.Centroids[point.ClusterNumber]);
            }

            return sum;
        }

        public List<ClusterStatistics> GetClusterStatistics()
        {
            List<ClusterStatistics> stat = new List<ClusterStatistics>();

            for (int i = 0; i < this.K; i++)
            {
                stat.Add(new ClusterStatistics
                {
                    ClusterNumber = i,
                    ClusterCount = clusterData.ClusterCount(i),
                    AverageX = clusterData.AverageX(i),
                    AverageY = clusterData.AverageY(i)
                });
            }

            return stat;
        }
    }
}
