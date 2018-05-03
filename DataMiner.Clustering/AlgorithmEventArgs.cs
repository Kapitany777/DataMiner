using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner.Clustering
{
    public delegate void AlgorithmEventHandler(object sender, AlgorithmEventArgs e);

    public class AlgorithmEventArgs : EventArgs
    {
        public int IterationNumber { get; private set; }
        public double SumOfErrors { get; private set; }
        public double SumOfSquaredErrors { get; private set; }

        public AlgorithmEventArgs(int iterationNumber, double sumOfErrors, double sumOfSquaredErrors)
        {
            this.IterationNumber = iterationNumber;
            this.SumOfErrors = sumOfErrors;
            this.SumOfSquaredErrors = sumOfSquaredErrors;
        }
    }
}
