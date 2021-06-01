using System;

namespace Simulation.Edit
{
    [Serializable]
    class Workplace : Venue
    {
        private WorkplaceType _type;
        private int _workerCapacity;

        public Workplace(GridCell position, float infectionRisk, WorkplaceType type, int workerCapacity) : base(position, infectionRisk)
        {
            _type = type;
            _workerCapacity = workerCapacity;
        }
    }
}