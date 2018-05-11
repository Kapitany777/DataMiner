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
    public class CsvReader : ClusterDataReader
    {
        private bool dataFound;
        private bool readingCompleted;
        private int dimensions;
        private bool hasHeader;
        private string[] header;
        private int lineCount;
        private int maxMatchCount;
        private int delIndex;
        private int encIndex;
        private string[] delimiter;
        private string[] enclosing;
        private string dataPattern;
        private string numberPatternDot;
        private string numberPatternDotComma;
        private Func<string, string, string, string> getRgxPattern;
        private Func<double[], ClusterPoint> getClusterPoint;

        public CsvReader(string fileName, ClusterData clusterData) : base(fileName, clusterData)
        {
        }
        
        public bool ReadingCompleted => readingCompleted;
        public bool DataFound => dataFound;
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

        public int Dimensions => dimensions;
        public int LineCount => lineCount;
        public string Delimiter => delimiter[delIndex];
        public string Enclosing => enclosing[encIndex];

        private void Init()
        {
            dimensions = 2;
            delimiter = new string[] { ";", ",", "\t", " " };
            enclosing = new string[] { "\"", "'", "" };
            dataPattern = ".*?";
            numberPatternDot = "(?:\\d+(?:\\.\\d+)?)?";
            numberPatternDotComma = "(?:\\d+(?:[\\.,]\\d+)?)?";
            getRgxPattern = (ptn, del, enc) => "(?<=(?:^|" + del + ")" + enc + ")(?<data>" + ptn + ")(?=" + enc + "(?:" + del + "|\n|\\Z))";
            getClusterPoint = data => new ClusterPoint(data[0], data[1]);
        }
       
        public override void ReadData()
        {
            Init();
            Test();
            if (!dataFound) return;

            using (StreamReader sr = new StreamReader(FileName, Encoding.UTF8, true))
            {
                MatchCollection matchColl;
                string numberPattern = enclosing[encIndex] == "" && delimiter[delIndex] == "," ? numberPatternDot : numberPatternDotComma;
                string regexNumberPattern = getRgxPattern(numberPattern, delimiter[delIndex], enclosing[encIndex]);
                string line;
                string match;
                double[] point = new double[dimensions];
                if (hasHeader)
                {
                    header = new string[dimensions];
                    line = sr.ReadLine();
                    string regexDataPattern = getRgxPattern(dataPattern, delimiter[delIndex], enclosing[encIndex]);
                    matchColl = Regex.Matches(line, regexDataPattern);

                    for (int mt = 0; mt < dimensions; mt++)
                    {
                        match = matchColl[mt].Groups["data"].Value;
                        header[mt] = match;
                    }
                }

                lineCount = 0;
                CultureInfo en_US = new CultureInfo("en-US");
                ClusterPoint cp;
                while ((line = sr.ReadLine()) != null)
                {
                    matchColl = Regex.Matches(line, regexNumberPattern);
                    if (matchColl.Count != maxMatchCount) return; // az adatok mennyisége nem megfelelő
                    for (int mt = 0; mt < dimensions; mt++)
                    {
                        match = matchColl[mt].Groups["data"].Value;
                        match = match.Replace(',', '.');
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
                    lineCount++;
                }
            }
            readingCompleted = true;
        }

        private void Test()
        {
            string[] testContent = new string[5];

            using (StreamReader sr = new StreamReader(FileName, Encoding.UTF8, true))
            {
                this.lineCount = 0;
                while (lineCount < testContent.Length && (testContent[lineCount] = sr.ReadLine()) != null)
                    lineCount++;
            }
            if (lineCount == 0) return;

            maxMatchCount = 0; //elég a maximumot követni, a közrezáró karakter néküli találatok a teszt végén vannak, ezért azokat csak akkor vesszük figyelembe, ha több találat van mint előtte.
            int matchCount = 0;
            bool headerCheck;
            for (int en = 0; en < enclosing.Length; en++)
            {
                for (int dl = 0; dl < delimiter.Length; dl++)
                {
                    headerCheck = false;
                    string rgxDataPattern = getRgxPattern(dataPattern, delimiter[dl], enclosing[en]);
                    string numberPattern = enclosing[en] == "" && delimiter[dl] == "," ? numberPatternDot : numberPatternDotComma;
                    string rgxNumberPattern = getRgxPattern(numberPattern, delimiter[dl], enclosing[en]);

                    MatchCollection matchColl;
                    matchColl = Regex.Matches(testContent[0], rgxDataPattern);
                    matchCount = matchColl.Count;

                    if (matchCount < dimensions) continue; // kevesebb adat a szükségesnél

                    matchColl = Regex.Matches(testContent[0], rgxNumberPattern);
                    if (matchCount > matchColl.Count) headerCheck = true; // az első sor nem (csak) számokból áll
                    else matchCount = matchColl.Count;

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
                        this.delIndex = dl;
                        this.encIndex = en;
                        this.hasHeader = headerCheck;
                        dataFound = true;
                    }
                }
            }
        }
    }
}