namespace Simulation.Runtime
{
    public class Hospital : Workplace
    {
        private Scale _scale;
        private WorkerAvailability _workerAvailability;

        public Hospital(Scale scale, WorkerAvailability workerAvailability, int workerCapacity, float infectionRisk)
            : base(Type.Hospital, workerCapacity, infectionRisk)
        {
            HospitalScale = scale;
            HospitalWorkerAvailability = workerAvailability;
        }

        public Scale HospitalScale { get => _scale; set => _scale = value; }
        public WorkerAvailability HospitalWorkerAvailability { get => _workerAvailability; set => _workerAvailability = value; }

        public enum Scale
        {
            Small,
            Medium,
            Large
        }

        public enum WorkerAvailability
        {
            Low,
            Medium,
            High
        }
    }
}
