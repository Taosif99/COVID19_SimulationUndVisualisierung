using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Simulation.Runtime
{
    public class HealthState
    {
        /*
        private bool _inHospital;
        private bool _intensiveCare;
        private DateTime _deadDate;
        private bool trigger;
        private bool _isDead;
        */

        private Person _person;

        private bool _willRecoverFromCoViD;
        private bool _willRecoverInHosptal;
        private bool _willDieInIntensiveCare;

        private int _deathDay;

        public bool WillRecoverFromCoViD { get => _willRecoverFromCoViD; set => _willRecoverFromCoViD = value; }
        public bool WillRecoverInHosptal { get => _willRecoverInHosptal; set => _willRecoverInHosptal = value; }
        public bool WillDieInIntensiveCare { get => _willDieInIntensiveCare; set => _willDieInIntensiveCare = value; }
        public int DeathDay { get => _deathDay; set => _deathDay = value; }

        public HealthState(Person person)
        {
            _person = person;

            float probabilityToRecover = Random.Range(0f, 1f);
            if (probabilityToRecover <= DefaultInfectionParameters.HealthPhaseParameters.RecoveringProbability)
            {
                _willRecoverFromCoViD = true;
            }
            else
            {

                _willRecoverFromCoViD = false;
                float probabilityToRecoverInHospital = Random.Range(0f, 1f);

                if (probabilityToRecoverInHospital <= DefaultInfectionParameters.HealthPhaseParameters.RecoveringInHospitalProbability)
                {
                    _willRecoverInHosptal = true;
                }
                else
                {
                    _willRecoverInHosptal = false;
                    float probabilityToSurviveIntensiveCare = Random.Range(0f, 1f);
                    if (probabilityToSurviveIntensiveCare <= DefaultInfectionParameters.HealthPhaseParameters.PersonSurvivesIntensiveCareProbability)
                    {
                        _willDieInIntensiveCare = false;

                    }
                    else
                    {
                        _willDieInIntensiveCare = true;
                        Debug.LogWarning("I will die !");
                    }

                }
            }
        }


        public void UpdateHealthState()
        {



            if (_willDieInIntensiveCare)
            {
                Simulation.Edit.AdjustableSimulationSettings settings = SimulationMaster.Instance.AdjustableSettings;


                if (_person.DaysSinceInfection >= (settings.IncubationTime + DefaultInfectionParameters.HealthPhaseParameters.DaysFromSymptomsBeginToDeath - 1))
                {
                    _person.IsDead = true;

                    //decrease amount of population 
                    Debug.LogWarning("RIP");
                }
                
            
            }
        
        
        }



        /// <summary>
        /// Checks if the person has symptoms and is alive. If these requirements are met, the health state will be updated. 
        /// The pre-illness is taken into account in the health state update.
        /// </summary>
        /// <param name="person">Called person in the simulation</param>

        /*
        public void UpdateHealthState(DateTime currentdate, DateTime infectionDate)
        {
            if (trigger == false) 
            {
                float hospitalProbability;
                hospitalProbability = Random.Range(0f, 1f);
                if (hospitalProbability <= Simulation.DefaultInfectionParameters.HealthPhaseParameters.RecoveringProbability)
                {
                    float recoverProbability = Random.Range(0f, 1f);
                    if (recoverProbability >= Simulation.DefaultInfectionParameters.HealthPhaseParameters.RecoveringInHospitalProbability)
                    {
                        _inHospital = true;
                    }
                    else
                    {
                        float surviveProbability = Random.Range(0f, 1f);
                        if (surviveProbability <= Simulation.DefaultInfectionParameters.HealthPhaseParameters.PersonSurvivesIntensiveCareProbability)
                        {
                            _intensiveCare = true;
                            int deadDay = Random.Range(1, Simulation.DefaultInfectionParameters.InfectionsPhaseParameters.AmountDaysSymptoms);
                            _deadDate = infectionDate.AddDays(deadDay);
                        }
                    }
                }
                trigger = true;
            }
            if (currentdate == infectionDate)
            {
                _isDead = true;
                Debug.Log("person is shahid");
            }
        }*/


    }
}
