using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner.Clustering
{
    public class ClusterPoint
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double XT { get; set; }
        public double YT { get; set; }
        public int ClusterNumber { get; private set; }

        public ClusterPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
            this.ClusterNumber = 0;
        }

        public override string ToString()
        {
            return $"X = {X}, Y = {Y}, Cluster = {ClusterNumber}";
        }
    }
}
