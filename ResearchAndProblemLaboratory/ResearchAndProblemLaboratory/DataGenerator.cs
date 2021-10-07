using MathNet.Numerics.Distributions;

namespace ResearchAndProblemLaboratory
{
    public static class DataGenerator
    {
        public static double[] GetErlangDistribution(int counter, int shape, double rate)
        {
            double[] values = new double[counter];
            Erlang.Samples(values, shape, rate);
            return values;
        }
    }
}
