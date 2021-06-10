using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

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
        
        public void SimulateEncounters(DateTime simulationDate)
        {
            foreach (Person p in _currentPeopleAtVenue)
            {
                if (p.InfectionState.HasFlag(Person.InfectionStates.Infected))
                {
                    continue;
                }
                
                foreach (Person i in _currentPeopleAtVenue)
                {
                    if (!p.InfectionState.HasFlag(Person.InfectionStates.Infectious))
                    {
                        continue;
                    }

                    float infectionProbability = InfectionRisk * (1 - (p.CarefulnessFactor + i.CarefulnessFactor) / 2);

                    if (Random.Range(0f, 1f) <= infectionProbability)
                    {
                        p.SetInfected(simulationDate);
                    }
                }
            }
        }

        public bool HasPersonHere(Person person) => _currentPeopleAtVenue.Contains(person);

        public void RemovePerson(Person person)
        {
            if (!HasPersonHere(person))
            {
                return;
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

        public HashSet<Person> GetPeopleAtVenue()
        {
            return _currentPeopleAtVenue;
        }
    }
}
