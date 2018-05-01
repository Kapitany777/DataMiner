using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner.Clustering
{
    public class ClusterPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double XT { get; set; }
        public double YT { get; set; }
        public int ClusterNumber { get; set; }

        public ClusterPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
            this.XT = x;
            this.YT = y;
            this.ClusterNumber = 0;
        }

        public ClusterPoint(ClusterPoint p, int clusterNumber)
        {
            this.X = p.X;
            this.Y = p.Y;
            this.XT = p.XT;
            this.YT = p.YT;
            this.ClusterNumber = clusterNumber;
        }

        public override string ToString()
        {
            return $"X = {X}, Y = {Y}, Cluster = {ClusterNumber}";
        }
    }
}
