using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace DataMiner.Clustering
{
    public static class Distance
    {
        public static double SquaredEuclidean(ClusterPoint p1, ClusterPoint p2) =>
            (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);

        public static double Euclidean(ClusterPoint p1, ClusterPoint p2) => Sqrt(SquaredEuclidean(p1, p2));

        public static double Manhattan(ClusterPoint p1, ClusterPoint p2) => Abs(p1.X - p2.X) + Abs(p1.Y - p2.Y);

        public static double Chebyshev(ClusterPoint p1, ClusterPoint p2) => Max(Abs(p1.X - p2.X), Abs(p1.Y - p2.Y));
    }
}
