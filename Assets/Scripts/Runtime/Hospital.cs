
namespace Assets.Scripts.Runtime
{
    class Hospital : Workplace
    {
        private Scale _scale;
        private WorkerAvailability _workerAvailability;

        public Hospital(Scale scale, WorkerAvailability workerAvailability, int workerCapacity, float infectionRisk)
            : base(Type.Hospital, workerCapacity, infectionRisk)
        {
            _scale = scale;
            _workerAvailability = workerAvailability;
        }

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
