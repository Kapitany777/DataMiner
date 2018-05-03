using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner.Clustering
{
    public class ClusterStatistics
    {
        public int ClusterNumber { get; set; }
        public int ClusterCount { get; set; }
        public double MinX { get; set; }
        public double MaxX { get; set; }
        public double AverageX { get; set; }
        public double StdDevX { get; set; }
        public double MinY { get; set; }
        public double MaxY { get; set; }
        public double AverageY { get; set; }
        public double StdDevY { get; set; }
    }
}
