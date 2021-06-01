using System;

namespace Simulation.Edit
{
    [Serializable]
    class Venue : Entity
    {
        private float _infectionRisk;

        public Venue(GridCell position, float infectionRisk) : base(position)
        {
            _infectionRisk = infectionRisk;
        }
    }
}
