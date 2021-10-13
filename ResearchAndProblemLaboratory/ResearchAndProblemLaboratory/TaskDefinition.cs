namespace ResearchAndProblemLaboratory
{
    public class TaskDefinition
    {
        public int  Id;
        public double TS;
        public double Tp;

        public TaskDefinition(int id, double ts, double tp)
        {
            Id = id;
            TS = ts;
            Tp = tp;
        }

        public override string ToString() =>
            $"Id: {Id}; TS: {TS}; Tp: {Tp}";
    }
}
