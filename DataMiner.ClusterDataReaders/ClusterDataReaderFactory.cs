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
                    CsvReader cr = new CsvReader(fileName, clusterData);
                    return cr.ReadingCompleted ? cr : null;

                case ".txt":
                    TxtReader tr = new TxtReader(fileName, clusterData);
                    return tr.ReadingCompleted ? tr : null;

                default:
                    return null;
            }
        }
    }
}
