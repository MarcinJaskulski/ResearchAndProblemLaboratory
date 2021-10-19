namespace ResearchAndProblemLaboratory
{
    public class TaskDefinition
    {
        public int  Id { get; set; }
        public double TS { get; set; }
        public double Tp { get; set; }
        public double Sdl { get; set; }
        public double RemainingTime { get; set; }

        public TaskDefinition(int id, double ts, double tp, double DTMax)
        {
            Id = id;
            TS = ts;
            Tp = tp;
            Sdl = ts + DTMax;
            RemainingTime = Tp;
        }

        public override string ToString() =>
            $"Id: {Id}; TS: {TS}; Tp: {Tp}";
    }
}
