using System;

namespace Simulation.Edit
{
    [Serializable]
    public class Hospital : Workplace
    {


        public HospitalScale Scale { get ; set; }
        public WorkerAvailability WorkerAvailability { get ; set; }

        public Hospital(GridCell position, float infectionRisk, WorkplaceType type, int workerCapacity, HospitalScale scale, WorkerAvailability workerAvailability) : base(position, infectionRisk, type, workerCapacity)
        {
            Scale = scale;
            WorkerAvailability = workerAvailability;
        }

      
    }
}
