using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAndProblemLaboratory
{
    class Program
    {
        const int Counter = 6;
        const int Phazes = 6;
        const double DTMax = 3;


        static void Main(string[] args)
        {
            var iterations = 1;

            for (var k = 1; k < 2; k += 1)
            {
                var avg_stats = new Stats();
                avg_stats.reset();
                for (var i = 0; i < iterations; i++)
                {
                    var stats = MainFunction(k);
                    avg_stats.latenes_avg += stats.latenes_avg;
                    avg_stats.tasks_on_time += stats.tasks_on_time;
                    avg_stats.delta_interval += stats.delta_interval;
                    avg_stats.task_size += stats.task_size;
                }
                avg_stats.tasks_on_time = avg_stats.tasks_on_time / iterations;
                avg_stats.latenes_avg = avg_stats.latenes_avg / iterations;
                avg_stats.delta_interval = avg_stats.delta_interval / iterations;
                avg_stats.task_size = avg_stats.task_size / iterations;

                Console.WriteLine($"Final: {avg_stats.tasks_on_time} {avg_stats.latenes_avg}");
                Console.WriteLine($"k: {k}, task_size: {avg_stats.task_size}");
                Console.WriteLine("\n");
                avg_stats.reset();
            }

        }

        static Stats MainFunction(int k)
        {
            //to nieważne
            //var deltaTaskLength = 0;
            //DataGenerator.avgTaskLength1 = avgTaskLength - deltaTaskLength;
            //DataGenerator.avgtaskLength2 = avgTaskLength + deltaTaskLength;


            var avgTaskLength = 0.95;
            var avgInterval = 0.2;
            var deltaInterval = 0;

            DataGenerator.avgInterval1 = avgInterval - deltaInterval;
            DataGenerator.avgInterval2 = avgInterval + deltaInterval;
            DataGenerator.erlangShape = 1;
            DataGenerator.avgTaskLengthErlang = avgTaskLength;

            if (avgInterval <= deltaInterval)
            {
                throw new InvalidOperationException("Delta musi być mniejsza niż średnia");
            }

            var tasks = DataGenerator.GenerateTasksWithErlang(Counter, Phazes, DTMax);
            var tasksCopy = new List<TaskDefinition>(tasks);

            var stats = ClassicalFifo.Run(tasks, Counter, DTMax);
            //var stats = ExtendedAlgorithm.Run(tasks, Counter, DTMax);

            var lambda = tasksCopy.Count / tasksCopy[tasksCopy.Count - 1].TS;

            var avgTp = tasksCopy.Average(x => x.Tp);
            var avgIntervalResult = tasksCopy.Average(x => x.Interval);
            var mi = 1 / avgTp;
            Console.WriteLine($"Obciążenie: {lambda / mi}");
            Console.WriteLine($"Współczynnik zmienności długości zadań: {StandardDeviation(tasksCopy.Select(x => x.Tp).ToArray()) / avgTp}");
            Console.WriteLine($"Współczynnik zmienności odstępów między zadaniami: {StandardDeviation(tasksCopy.Select(x => x.Interval).ToArray()) / avgIntervalResult}");
            stats.delta_interval = StandardDeviation(tasksCopy.Select(x => x.Interval).ToArray()) / avgIntervalResult;
            stats.task_size = StandardDeviation(tasksCopy.Select(x => x.Tp).ToArray()) / avgTp;
            return stats;
        }

        private static double StandardDeviation(double[] someDoubles)
        {
            double average = someDoubles.Average();
            double sumOfSquaresOfDifferences = someDoubles.Select(val => (val - average) * (val - average)).Sum();
            return Math.Sqrt(sumOfSquaresOfDifferences / someDoubles.Length);
        }
    }
}
