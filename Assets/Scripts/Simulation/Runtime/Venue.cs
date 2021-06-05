using System;
using System.Collections.Generic;

namespace Simulation.Runtime
{
    abstract class Venue : Entity
    {
        private HashSet<Person> _currentPeopleAtVenue = new HashSet<Person>();

        protected Venue(Edit.Venue editorEntity) : base(editorEntity)
        {
            InfectionRisk = editorEntity.InfectionRisk;
        }

        public float InfectionRisk { get; }
        
        public void SimulateEncounters()
        {
            throw new NotImplementedException();
        }
    }
}
