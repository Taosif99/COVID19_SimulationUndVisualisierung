namespace Simulation.Runtime
{
    public class Workplace : Venue
    {
        private Type _type;
        private int _workerCapacity;

        public Workplace(Type type, int workerCapacity, float infectionRisk) : base(infectionRisk)
        {
            WorkType = type;
            WorkerCapacity = workerCapacity;
        }

        public Type WorkType { get => _type; set => _type = value; }
        public int WorkerCapacity { get => _workerCapacity; set => _workerCapacity = value; }

        public enum Type
        {
            Other,
            Store,
            Hospital
        }
    }
}
