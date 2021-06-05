using System;
using System.Collections.Generic;

namespace Simulation.Runtime
{
    abstract class Venue : Entity
    {
        private HashSet<Person> _currentPeopleAtVenue = new HashSet<Person>();

        protected Venue(Edit.Venue editorEntity) : base(editorEntity)
        {
            throw new NotImplementedException();
        }

        public float InfectionRisk { get; }
        
        public void SimulateEncounters()
        {
            throw new NotImplementedException();
        }
    }
}
