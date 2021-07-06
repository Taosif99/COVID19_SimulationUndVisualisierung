using System;

namespace Simulation.Edit
{
    [Serializable]
   public class Workplace : Venue
    {
        public WorkplaceType Type { get; set; }

        public int WorkerCapacity { get; set; }

        public bool CoronaTestsEnabled { get; set; }

        public Workplace(GridCell position, float infectionRisk, WorkplaceType type, int workerCapacity, bool coronaTestsEnabled) : base(position, infectionRisk)
        {
            Type= type;
            WorkerCapacity = workerCapacity;
            this.CoronaTestsEnabled = coronaTestsEnabled;
        }
    }
}