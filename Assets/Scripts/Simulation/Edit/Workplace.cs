using System;

namespace Simulation.Edit
{
    [Serializable]
   public class Workplace : Venue
    {


        public WorkplaceType Type { get; set; }


        public int WorkerCapacity { get; set; }

        public Workplace(GridCell position, float infectionRisk, WorkplaceType type, int workerCapacity) : base(position, infectionRisk)
        {
            Type= type;
            WorkerCapacity = workerCapacity;
        }
    }
}