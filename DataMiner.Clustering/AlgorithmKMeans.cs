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
            InitializeCentroids();

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
            }
        }
    }
}
