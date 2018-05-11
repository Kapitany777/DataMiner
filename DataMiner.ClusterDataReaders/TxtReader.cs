using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataMiner.Clustering;
using System.IO;
using System.Globalization;

namespace DataMiner.ClusterDataReaders
{
    public class TxtReader : ClusterDataReader
    {
        private int dimensions;
        private string pattern;
        private Func<double[], ClusterPoint> getClusterPoint;
        private bool readingCompleted;

        public bool ReadingCompleted => readingCompleted;

        public TxtReader(string fileName, ClusterData clusterData) : base(fileName, clusterData)
        {
        }
        private void Init()
        {
            dimensions = 2;
            pattern = "(?<data>[^ \n]+)";
            getClusterPoint = data => new ClusterPoint(data[0], data[1]);
        }
        public override void ReadData()
        {
            Init();

            using (StreamReader sr = new StreamReader(this.FileName))
            {
                string line;
                MatchCollection matchColl;
                string match;
                double[] point;
                CultureInfo en_US = new CultureInfo("en-US");
                ClusterPoint cp;

                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Replace(',', '.');
                    matchColl = Regex.Matches(line, pattern);
                    if (matchColl.Count < dimensions) return;

                    point = new double[dimensions];

                    for (int mt = 0; mt < dimensions; mt++)
                    {
                        match = matchColl[mt].Groups["data"].Value;
                        try
                        {
                            point[mt] = double.Parse(match, en_US);
                        }
                        catch (Exception ex) when (ex is ArgumentNullException || ex is FormatException || ex is OverflowException)
                        {
                            return;
                        }
                    }
                    cp = getClusterPoint(point);
                    clusterData.Add(cp);
                }
            }
            readingCompleted = true;
        }
    }
}