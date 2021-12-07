namespace ResearchAndProblemLaboratory
{
    public class Stats
    {
        public double latenes_avg { get; set; }
        public double tasks_on_time { get; set; }
        public double delta_interval { get; set; }
        public double task_size { get; set; }

        public void reset()
        {
            latenes_avg = 0.0;
            tasks_on_time = 0.0;
            delta_interval = 0.0;
            task_size = 0.0;
        }
    }
}
