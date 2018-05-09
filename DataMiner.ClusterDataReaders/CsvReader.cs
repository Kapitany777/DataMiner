using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using DataMiner.Clustering;
using System.Globalization;


namespace DataMiner.ClusterDataReaders
{
    public class CsvReader
    {
        private int dimensions;
        private bool hasHeader;
        private string[] header;
        private string path;
        private int lineCount;
        private int maxMatchCount;
        private ClusterData clusterData;
        private int delimFound;
        private int enclFound;
        private string[] delimiter;
        private string[] enclosing;
        private string dataPattern;
        private string numberPattern;
        private Func<string, string, string, string> getPattern;
        private Func<double[], ClusterPoint> getClusterPoint;

        public CsvReader(string path)
        {
            dimensions = 2;
            this.path = path;
            delimiter = new string[] { ";", ",", "\t", " " };
            enclosing = new string[] { "\"", "'", "" };
            dataPattern = ".*?";
            numberPattern = "(?:\\d+(?:\\.\\d+)?)?";
            getPattern = (ptn, del, enc) => "(?<=(?:^|" + del + ")" + enc + ")(?<data>" + ptn + ")(?=" + enc + "(?:" + del + "|\n|\\Z))";
            getClusterPoint = data => new ClusterPoint(data[0], data[1]);
            Read();
        }
        public ClusterData ClusterData => clusterData;
        public bool HasHeader => hasHeader;
        public string[] Header
        {
            get
            {
                if (hasHeader) return header;
                else
                {
                    header = new string[dimensions];
                    for (int i = 0; i < header.Length; i++) header[i] = "---";
                    return header;
                }
            }
        }
        public int LineCount => lineCount;
        public string Delimiter => delimiter[delimFound];
        public string Enclosing => enclosing[enclFound];

        private void Read()
        {
            if (!Test()) return;

            ClusterData returnData = new ClusterData();

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8, true))
            {
                MatchCollection matchColl;
                string regexNumberPattern = getPattern(numberPattern, delimiter[delimFound], enclosing[enclFound]);
                string line;
                string match;
                double[] point = new double[dimensions];
                if (hasHeader)
                {
                    header = new string[dimensions];
                    line = sr.ReadLine();
                    string regexDataPattern = getPattern(dataPattern, delimiter[delimFound], enclosing[enclFound]);
                    matchColl = Regex.Matches(line, regexDataPattern);

                    for (int mt = 0; mt < dimensions; mt++)
                    {
                        match = matchColl[mt].Groups["data"].Value;
                        header[mt] = match;
                    }
                }
                while ((line = sr.ReadLine()) != null)
                {
                    matchColl = Regex.Matches(line, regexNumberPattern);
                    if (matchColl.Count != maxMatchCount) return; // az adatok mennyisége nem megfelelő
                    for (int mt = 0; mt < dimensions; mt++)
                    {
                        match = matchColl[mt].Groups["data"].Value;
                        try
                        {
                            point[mt] = double.Parse(match, new CultureInfo("en-US"));
                        }
                        catch (Exception ex) when (ex is ArgumentNullException || ex is FormatException || ex is OverflowException)
                        {
                            return;
                        }
                    }
                    ClusterPoint cp = getClusterPoint(point);
                    returnData.Add(cp);
                }
            }
            this.clusterData = returnData;
        }
        private bool Test()
        {
            bool validMatchFound = false;
            string[] testContent = new string[5];

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8, true))
            {
                this.lineCount = 0;
                while (lineCount < testContent.Length && (testContent[lineCount] = sr.ReadLine()) != null)
                    lineCount++;
            }
            if (lineCount == 0) return false;

            maxMatchCount = 0; //elég a maximumot követni, a közrezáró karakter néküli találatok a teszt végén vannak, ezért azokat csak akkor vesszük figyelembe, ha több találat van mint előtte.
            int matchCount = 0;
            bool headerCheck;
            for (int en = 0; en < enclosing.Length; en++)
            {
                for (int dl = 0; dl < delimiter.Length; dl++)
                {
                    headerCheck = false;
                    string rgxDataPattern = getPattern(dataPattern, delimiter[dl], enclosing[en]);
                    string rgxNumberPattern = getPattern(numberPattern, delimiter[dl], enclosing[en]);

                    MatchCollection matchColl;
                    matchColl = Regex.Matches(testContent[0], rgxDataPattern);
                    matchCount = matchColl.Count;

                    if (matchCount < dimensions) continue; // kevesebb adat a szükségesnél

                    matchColl = Regex.Matches(testContent[0], rgxNumberPattern);
                    if (matchCount > matchColl.Count) headerCheck = true; // az első sor nem (csak) számokból áll

                    for (int ln = 1; ln < lineCount; ln++)
                    {
                        matchColl = Regex.Matches(testContent[ln], rgxNumberPattern);

                        if (matchCount != matchColl.Count) // eltérő számú találat soronként
                        {
                            matchCount = 0;
                            break;
                        }
                    }
                    if (maxMatchCount < matchCount)
                    {
                        this.maxMatchCount = matchCount;
                        this.delimFound = dl;
                        this.enclFound = en;
                        this.hasHeader = headerCheck;
                        validMatchFound = true;
                    }
                }
            }
            return validMatchFound;
        }
    }
}