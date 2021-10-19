using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace ResearchAndProblemLaboratory
{
    public static class DataGenerator
    {
        public static IEnumerable<TaskDefinition> GenerateTasks(int numberOfTasks, int phazes, double DTMax)
        {
            double timer = 0;
            var result = new List<TaskDefinition>();
            int taskCounter = 0;

            for (int i = 0; i < 5; i++)
            {
                var erlangs = GetErlangDistribution(numberOfTasks / phazes, 2, 0.5);
                var expontentials =
                    i % 2 == 0 ?
                    GetExponentialDistribution(numberOfTasks / phazes, 0.5) :
                    GetExponentialDistribution(numberOfTasks / phazes, 0.2);

                foreach (var (erlang, expon) in erlangs.Zip(expontentials))
                {
                    //timer += expon;
                    result.Add(new TaskDefinition(taskCounter++, erlang, expon, DTMax));
                    Console.WriteLine(result.Last().ToString());
                }
            }

            return result;
        }

        private static double[] GetErlangDistribution(int counter, int shape, double rate)
        {
            double[] values = new double[counter];
            Erlang.Samples(new Random(), values, shape, rate);
            return values.Select(x => Math.Round(x, 2)).ToArray();
        }

        private static double[] GetExponentialDistribution(int counter, double rate)
        {
            double[] values = new double[counter];
            Exponential.Samples(new Random(), values, rate);
            return values.Select(x=>Math.Round(x,2)).ToArray();
        }
    }
}
