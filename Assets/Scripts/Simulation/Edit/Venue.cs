using System;

namespace Simulation.Edit
{
    [Serializable]
    public class Venue : Entity
    {

        public float InfectionRisk { get; set; }

        public Venue(GridCell position, float infectionRisk) : base(position)
        {
            InfectionRisk = infectionRisk;
        }
    }
}
