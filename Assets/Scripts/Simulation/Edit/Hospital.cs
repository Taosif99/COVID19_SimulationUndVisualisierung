using System;

namespace Simulation.Edit
{
    [Serializable]
    public class Hospital : Workplace
    {
        private HospitalScale _scale;
        private WorkerAvailability _workerAvailability;

        public HospitalScale Scale { get => _scale; set => _scale = value; }
        public WorkerAvailability WorkerAvailability { get => _workerAvailability; set => _workerAvailability = value; }

        public Hospital(GridCell position, float infectionRisk, WorkplaceType type, int workerCapacity, HospitalScale scale, WorkerAvailability workerAvailability) : base(position, infectionRisk, type, workerCapacity)
        {
            Scale = scale;
            WorkerAvailability = workerAvailability;
        }

      
    }
}
