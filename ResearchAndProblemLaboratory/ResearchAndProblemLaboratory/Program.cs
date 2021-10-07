using System;

namespace ResearchAndProblemLaboratory
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var a = DataGenerator.GetErlangDistribution(10, 1, 5);
        }
    }
}
