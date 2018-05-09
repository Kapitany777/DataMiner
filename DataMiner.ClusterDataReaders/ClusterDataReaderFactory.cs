using DataMiner.Clustering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner.ClusterDataReaders
{
    public static class ClusterDataReaderFactory
    {
        public static ClusterDataReader GetClusterDataReader(string fileName, ClusterData clusterData)
        {
            string ext = Path.GetExtension(fileName).ToLower();

            switch (ext)
            {
                case ".csv":
                    //return new CsvDataReader(fileName, clusterData);
                    return new CsvReader(fileName, clusterData);

                case ".txt":
                    return new TxtDataReader(fileName, clusterData);

                default:
                    return null;
            }
        }
    }
}
