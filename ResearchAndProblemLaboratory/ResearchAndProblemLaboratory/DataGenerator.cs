using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;

namespace ResearchAndProblemLaboratory
{
    public static class DataGenerator
    {
        public static List<double> GetErlangDistribution(int counter, int shape, double rate)
        {
            double[] values = new double[counter];
            Erlang.Samples(values, shape, rate);
            return values.ToList();
        }
    }
}
