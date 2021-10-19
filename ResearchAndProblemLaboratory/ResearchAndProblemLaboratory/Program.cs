using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAndProblemLaboratory
{
    class Program
    {
        const int Counter = 250;
        const int Phazes = 5;
        const double DTMax = 10;

        static void Main(string[] args)
        {

            var tasks = DataGenerator.GenerateTasks(Counter, Phazes, DTMax);


            double timer = 0;
            var worker = new Worker();
            var basicQueue = new List<TaskDefinition>();
            var poorQueue = new List<TaskDefinition>();

            while (true)
            {
                if (tasks.Count == 0 && 
                    basicQueue.Count ==0 && 
                    poorQueue.Count == 0 &&
                    !worker.IsBusy)
                    break;

                var tasksArrived = tasks.Where(x => x.TS == timer).ToList();

                basicQueue.AddRange(tasksArrived);
                tasksArrived.ForEach(t => tasks.Remove(t));

                worker.Execute();
                //foreach (var task in basicQueue)
                for (int i = 0; i < basicQueue.Count; i++)
                {
                    var task = basicQueue[i];
                    if (task.Sdl >= timer)
                    {
                        if (!worker.IsBusy)
                        {
                            worker.StartTask(task, timer, basicQueue);
                        }
                        else if (worker.IsTaskFromPoorQueue)
                        {
                            worker.SendTaskToPoorQueue(poorQueue);
                            worker.StartTask(task, timer, basicQueue);
                        }
                    }
                    else
                    {
                        basicQueue.Remove(task);
                        poorQueue.Add(task);
                    }
                }

                if (!worker.IsBusy && poorQueue.Count > 0)
                {
                    var taskToStart = poorQueue.First();
                    worker.StartTask(taskToStart, timer, poorQueue, true);
                }

                timer = Math.Round(timer + 0.01, 2);
            }

        }

        public class Worker
        {
            public bool IsTaskFromPoorQueue { get; set; }
            public bool IsBusy { get; set; }
            public TaskDefinition CurrentTask { get; set; }
            public double CurrentTaskEndTime { get; set; }

            private static int _endedTasksCounter = 0;

            public Worker()
            {
                IsBusy = false;
            }

            public void StartTask(TaskDefinition task, double timer, List<TaskDefinition> queue, bool isTaskFromPoorQueue = false)
            {
                CurrentTask = task;
                CurrentTaskEndTime = timer + task.RemainingTime;
                IsBusy = true;
                IsTaskFromPoorQueue = isTaskFromPoorQueue;
                queue.Remove(task);
            }

            public void SendTaskToPoorQueue(List<TaskDefinition> poorQueue)
            {
                poorQueue.Insert(0, CurrentTask);
                CurrentTask = null;
            }

            public void Execute()
            {
                if (CurrentTask != null)
                {
                    CurrentTask.RemainingTime = Math.Round(CurrentTask.RemainingTime - 0.01, 2);
                    if (CurrentTask.RemainingTime == 0)
                    {
                        IsBusy = false;
                        IsTaskFromPoorQueue = false;

                        Console.WriteLine($"Zakończył się: {CurrentTask.Id}; Task nr: {++_endedTasksCounter}");
                        CurrentTask = null;

                    }
                }
            }
        }
    }
}
