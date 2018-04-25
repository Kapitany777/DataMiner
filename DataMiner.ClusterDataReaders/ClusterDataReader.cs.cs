using DataMiner.Clustering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner.ClusterDataReaders
{
    public abstract class ClusterDataReader
    {
        public string FileName { get; }
        protected ClusterData clusterData { get; }

        public ClusterDataReader(string fileName, ClusterData clusterData)
        {
            this.FileName = fileName;
            this.clusterData = clusterData;

            ReadData();
        }

        public abstract void ReadData();
    }
}
