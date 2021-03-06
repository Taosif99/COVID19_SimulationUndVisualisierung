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
            InfectionRiskFactor = editorEntity.InfectionRisk;
        }

        public float InfectionRiskFactor { get; }
        
        public void SimulateEncounters(DateTime simulationDate, float maskFactor)
        {
            //Debug.Log("Used Mask Factor: " + maskFactor);

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


                    /*
                     * At this place the infection of a person is determined. The infection is dependant on average values and
                     * factors. The lerp is used in order to go above the average.
                     * 
                     * The carefulness could have more influence than one thinks.
                     * 
                     * Here the infection logic can be improved/edited
                     */
                   
                    float carefulnessInfectionRiskFactor = MathC.MapLinearToFactor(2f, (p.CarefulnessFactor + i.CarefulnessFactor) / 2);
                    float venueInfectionRiskFactor = MathC.MapLinearToFactor(2f, this.InfectionRiskFactor);
                 

                    float infectionProbability = p.InfectionRisk *
                                                 venueInfectionRiskFactor *
                                                 carefulnessInfectionRiskFactor *
                                                 GeneralInfectionProbabilityFactor *
                                                 maskFactor;
                    
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
