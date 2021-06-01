using System;

namespace Simulation.Edit
{
    [Serializable]
    class Hospital : Workplace
    {
        private HospitalScale _scale;
        private WorkerAvailability _workerAvailability;

        public Hospital(GridCell position, float infectionRisk, WorkplaceType type, int workerCapacity, HospitalScale scale, WorkerAvailability workerAvailability) : base(position, infectionRisk, type, workerCapacity)
        {
            _scale = scale;
            _workerAvailability = workerAvailability;
        }
    }
}
