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
            _infectionRisk = infectionRisk;
        }

        public void SimulateEncounters()
        {
            throw new NotImplementedException();
        }
    }
}
