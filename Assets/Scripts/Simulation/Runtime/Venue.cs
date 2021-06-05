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

        public bool HasPersonHere(Person person) => _currentPeopleAtVenue.Contains(person);

        public void RemovePerson(Person person)
        {
            if (!HasPersonHere(person))
            {
                throw new InvalidOperationException("Person is not at this venue.");
            }

            person.CurrentLocation = null;
            _currentPeopleAtVenue.Remove(person);
        }

        public void MovePersonHere(Person person)
        {
            if (HasPersonHere(person))
            {
                return;
            }

            person.CurrentLocation?.RemovePerson(person);

            person.CurrentLocation = this;
            _currentPeopleAtVenue.Add(person);
        }
    }
}
