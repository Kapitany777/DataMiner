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
            (p1.XT - p2.XT) * (p1.XT - p2.XT) + (p1.YT - p2.YT) * (p1.YT - p2.YT);

        public static double Euclidean(ClusterPoint p1, ClusterPoint p2) => Sqrt(SquaredEuclidean(p1, p2));

        public static double Manhattan(ClusterPoint p1, ClusterPoint p2) => Abs(p1.XT - p2.XT) + Abs(p1.YT - p2.YT);

        public static double Chebyshev(ClusterPoint p1, ClusterPoint p2) => Max(Abs(p1.XT - p2.XT), Abs(p1.YT - p2.YT));
    }
}
