using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace ResearchAndProblemLaboratory
{
    public static class DataGenerator
    {
        public static double avgTaskLengthErlang;
        public static double avgTaskLength1;
        public static double avgtaskLength2;
        public static double avgInterval2;
        public static double avgInterval1;
        public static int erlangShape;
        public static double drawPossibility;

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

        public static List<TaskDefinition> GenerateTasksWithErlang(int numberOfTasks, int phazes, double DTMax)
        {
            double timer = 0;
            var result = new List<TaskDefinition>();
            int taskCounter = 0;
            var taskLengths = GetErlangDistribution(numberOfTasks, erlangShape, erlangShape / avgTaskLengthErlang);
            var allIntervals = new List<double>();
            var random = new Random();
            for (int i = 0; i < phazes; i++)
            {
                for(int j = 0; j < numberOfTasks/phazes; j++)
                {
                    var isFromFirstAvg = random.Next(1, 101) < drawPossibility * 100;
                    if (i % 2 == 0)
                    {
                        if (isFromFirstAvg)
                        {
                            allIntervals.Add(Exponential.Sample(random, 1 / avgInterval1));
                        }
                        else
                        {
                            allIntervals.Add(Exponential.Sample(random, 1 / avgInterval2));
                        }
                    }
                    else
                    {
                        if (isFromFirstAvg)
                        {
                            allIntervals.Add(Exponential.Sample(random, 1 / avgInterval2));
                        }
                        else
                        {
                            allIntervals.Add(Exponential.Sample(random, 1 / avgInterval1));
                        }
                    }
                }
                //var intervals =
                //    i % 2 == 0 ?
                //    GetExponentialDistribution(numberOfTasks / phazes, 1 / avgInterval1) :
                //    GetExponentialDistribution(numberOfTasks / phazes, 1 / avgInterval2);
                //allIntervals.AddRange(intervals);
            }

            foreach (var (taskLength, interval) in taskLengths.Zip(allIntervals))
            {
                var previousTimer = timer;
                timer = Math.Round(timer + interval, 2);
                result.Add(new TaskDefinition(taskCounter++, timer, taskLength, DTMax, timer - previousTimer));
                Console.WriteLine(result.Last().ToString());
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
