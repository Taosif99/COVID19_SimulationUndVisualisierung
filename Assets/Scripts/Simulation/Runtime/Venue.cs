using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Simulation.Runtime
{
    public abstract class Venue : Entity
    {
        private const float GeneralInfectionProbabilityFactor = 0.1f;
        
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
                    if (!i.InfectionState.HasFlag(Person.InfectionStates.Infectious) || i.IsInHospitalization)
                    {
                        continue;
                    }

                    // TODO CONSIDER MASK FACTORS
                    float linearInterpolatedCarefulnessFactor = Mathf.Lerp(1.5f, 0.5f, (p.CarefulnessFactor + i.CarefulnessFactor) / 2);
                   
                    float infectionProbability = this.InfectionRisk * linearInterpolatedCarefulnessFactor * GeneralInfectionProbabilityFactor * p.InfectionRiskFactor;
                    
                    //Debug.Log($"Potential infection at {this} with probability {infectionProbability}");

                    if (Random.Range(0f, 1f) <= infectionProbability)
                    {
                        p.SetInfected(simulationDate);
                        Debug.Log("Person was infected.");
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
