using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchAndProblemLaboratory
{
    public static class ClassicalFifo
    {
        public static Stats Run(List<TaskDefinition> taskDefinitions, int counter, double dtMax)
        {
            double timer = 0;
            Stats stats = new Stats();

            foreach (var task in taskDefinitions)
            {
                if (timer >= task.TS)
                    timer += task.Tp;
                else
                    timer = task.TS + task.Tp;

                // Stats
                double latenes = timer - (task.TS + task.Tp + dtMax);
                if (latenes > 0)
                    stats.latenes_avg += latenes;
                else
                    stats.tasks_on_time++;
            }

            stats.latenes_avg /= counter;

            Console.WriteLine($"Timer: {timer}");
            return stats;
        }
    }
}
