using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace ResearchAndProblemLaboratory
{
    public static class DataGenerator
    {
        public static double avgTaskLength1;
        public static double avgtaskLength2;
        public static double avgInterval2;
        public static double avgInterval1;
        public static List<TaskDefinition> GenerateTasks(int numberOfTasks, int phazes, double DTMax)
        {
            double timer = 0;
            var result = new List<TaskDefinition>();
            int taskCounter = 0;

            for (int i = 0; i < phazes; i++)
            {
                var taskLengths =
                    i % 2 == 0 ?
                    GetExponentialDistribution(numberOfTasks / phazes, 1 / avgTaskLength1) :
                    GetExponentialDistribution(numberOfTasks / phazes, 1 / avgtaskLength2);
                var intervals =
                    i % 2 == 0 ?
                    GetExponentialDistribution(numberOfTasks / phazes, 1 / avgInterval1) :
                    GetExponentialDistribution(numberOfTasks / phazes, 1 / avgInterval2);

                foreach (var (taskLength, interval) in taskLengths.Zip(intervals))
                {
                    var previousTimer = timer;
                    timer = Math.Round(timer + interval, 2);
                    result.Add(new TaskDefinition(taskCounter++, timer, taskLength, DTMax, timer - previousTimer));
                    Console.WriteLine(result.Last().ToString());
                }
            }

            return result;
        }

        public static List<TaskDefinition> GenerateTasks(int numberOfTasks, int phazes, double DTMax, double relativeSizeOfSubset1, double relativeSizeOfSubset2)
        {
            double timer = 0;
            var result = new List<TaskDefinition>();
            int taskCounter = 0;

            for (int i = 0; i < 5; i++)
            {
                var taskLengths =
                    i % 2 == 0 ?
                    GetExponentialDistribution(numberOfTasks / phazes, 1 / avgTaskLength1) :
                    GetExponentialDistribution(numberOfTasks / phazes, 1 / avgtaskLength2);
                var intervals =
                    i % 2 == 0 ?
                    GetExponentialDistribution(numberOfTasks / phazes, 1 / avgInterval1) :
                    GetExponentialDistribution(numberOfTasks / phazes, 1 / avgInterval2);

                foreach (var (taskLength, interval) in taskLengths.Zip(intervals))
                {
                    var previousTimer = timer;
                    timer = Math.Round(timer + interval, 2);
                    result.Add(new TaskDefinition(taskCounter++, timer, taskLength, DTMax, timer - previousTimer));
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
            return values.Select(x => Math.Round(x, 2)).ToArray();
        }
    }
}
