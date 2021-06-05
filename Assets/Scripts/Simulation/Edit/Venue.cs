using System;

namespace Simulation.Edit
{
    [Serializable]
    public abstract class Venue : Entity
    {
        public float InfectionRisk { get; set; }

        protected Venue(GridCell position, float infectionRisk) : base(position)
        {
            InfectionRisk = infectionRisk;
        }
    }
}
