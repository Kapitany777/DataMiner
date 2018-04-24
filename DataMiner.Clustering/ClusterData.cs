using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner.Clustering
{
    public class ClusterData
    {
        private List<ClusterPoint> clusterData;

        public ClusterPoint this[int index]
        {
            get { return clusterData[index]; }
        }

        public ClusterData()
        {
            clusterData = new List<ClusterPoint>();
        }

        public void Add(ClusterPoint point)
        {
            clusterData.Add(point);
        }
    }
}
