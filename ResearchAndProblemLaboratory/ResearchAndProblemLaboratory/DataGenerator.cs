using System;
using MathNet.Numerics.Distributions;

namespace ResearchAndProblemLaboratory
{
    public static class DataGenerator
    {
        public static double[] GetErlangDistribution(int counter, int shape, double rate)
        {
            double[] values = new double[counter];
            Erlang.Samples(new Random(), values, shape, rate);
            return values;
        }

        public static double[] GetExponentialDistribution(int counter, double rate)
        {
            double[] values = new double[counter];
            Exponential.Samples(new Random(), values, rate);
            return values;
        }
    }
}
