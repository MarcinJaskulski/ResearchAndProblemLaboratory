using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAndProblemLaboratory
{
    class Program
    {
        const int Counter = 250;
        const int Phazes = 5;

        static void Main(string[] args)
        {
            double timer = 0;
            var result = new List<TaskDefinition>();
            int taskCounter = 0;

            for (int i = 0; i < 5; i++)
            {
                var erlangs = DataGenerator.GetErlangDistribution(Counter / Phazes, 2, 0.5);
                var expontentials = 
                    i%2 == 0 ?
                    DataGenerator.GetExponentialDistribution(Counter / Phazes, 0.5) :
                    DataGenerator.GetExponentialDistribution(Counter / Phazes, 0.2);

                foreach (var (erlang, expon) in erlangs.Zip(expontentials))
                {
                    timer += expon;
                    result.Add(new TaskDefinition(taskCounter++, erlang, timer));
                    Console.WriteLine(result.Last().ToString());
                }
            }

        }
    }
}
