using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMiner.Clustering;
using System.IO;

namespace DataMiner.ClusterDataReaders
{
    public class TxtDataReader : ClusterDataReader
    {
        public TxtDataReader(string fileName, ClusterData clusterData) : base(fileName, clusterData)
        {
        }

        public override void ReadData()
        {
            using (StreamReader sr = new StreamReader(this.FileName))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    string[] values = line.Split(' ');

                    this.clusterData.Add(new ClusterPoint(double.Parse(values[0]), double.Parse(values[values.Length - 1])));
                }
            }
        }
    }
}
