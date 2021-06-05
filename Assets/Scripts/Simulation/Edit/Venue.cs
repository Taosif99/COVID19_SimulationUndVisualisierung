using System;

namespace Simulation.Edit
{
    [Serializable]
    public class Venue : Entity
    {
        private float _infectionRisk;

        public float InfectionRisk
          {
                get { return _infectionRisk; }
                set { _infectionRisk= value; }
          }
       

        public Venue(GridCell position, float infectionRisk) : base(position)
        {
            _infectionRisk = infectionRisk;
        }
    }
}
