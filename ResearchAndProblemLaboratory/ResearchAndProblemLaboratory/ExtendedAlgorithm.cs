using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchAndProblemLaboratory
{
    public class ExtendedAlgorithm
    {
        public static Stats Run(List<TaskDefinition> taskDefinitions, int counter, double dtMax)
        {
            var tasksCopy = new List<TaskDefinition>(taskDefinitions);

            double timer = 0;
            var worker = new Worker();
            var basicQueue = new List<TaskDefinition>();
            var poorQueue = new List<TaskDefinition>();

            while (true)
            {
                if (taskDefinitions.Count == 0 &&
                    basicQueue.Count == 0 &&
                    poorQueue.Count == 0 &&
                    !worker.IsBusy)
                    break;

                var tasksArrived = taskDefinitions.Where(x => x.TS == timer).ToList();

                basicQueue.AddRange(tasksArrived);
                tasksArrived.ForEach(t => taskDefinitions.Remove(t));

                worker.Execute(counter, dtMax);
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
            return worker.stats_global;
        }

        public class Worker
        {
            public bool IsTaskFromPoorQueue { get; set; }
            public bool IsBusy { get; set; }
            public TaskDefinition CurrentTask { get; set; }
            public double CurrentTaskEndTime { get; set; }
            public Stats stats_global { get; set; }

            private static int _endedTasksCounter = 0;

            private double latenes_avg = 0;

            private double tasks_on_time = 0;

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

            public void Execute(int counter, double dtMax)
            {
                if (CurrentTask != null)
                {
                    CurrentTask.RemainingTime = Math.Round(CurrentTask.RemainingTime - 0.01, 2);
                    if (CurrentTask.RemainingTime == 0)
                    {
                        double latenes = CurrentTaskEndTime - (CurrentTask.TS + CurrentTask.Tp + dtMax);
                        if (latenes > 0)
                        {
                            latenes_avg += latenes;

                        }
                        else
                        {
                            tasks_on_time++;
                        }
                        IsBusy = false;
                        IsTaskFromPoorQueue = false;
                        Console.Title = $"{++_endedTasksCounter}";
                        //Console.WriteLine($"Zakończył się: {CurrentTask.Id}; Task nr: {++_endedTasksCounter}");
                        if (_endedTasksCounter == counter)
                        {
                            //Console.WriteLine($"on_time: {tasks_on_time / Counter}; avg: {latenes_avg / Counter}");
                            _endedTasksCounter = 0;
                            var stats_local = new Stats();
                            stats_local.latenes_avg = latenes_avg / counter;
                            stats_local.tasks_on_time = tasks_on_time / counter;
                            stats_global = stats_local;
                        }
                        CurrentTask = null;

                    }
                }
            }
        }
    }
}
