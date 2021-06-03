using System;
using System.Collections.Generic;

namespace Simulation.Runtime
{
    public class Venue : Entity
    {
        private float _infectionRisk;
        private HashSet<Person> _currentPeopleAtVenue = new HashSet<Person>();

        public Venue(float infectionRisk)
        {
            InfectionRisk = infectionRisk;
        }

        public float InfectionRisk { get => _infectionRisk; set => _infectionRisk = value; }
        

        public void SimulateEncounters()
        {
            throw new NotImplementedException();
        }
    }
}
