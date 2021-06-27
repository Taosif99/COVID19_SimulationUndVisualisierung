using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Simulation.Runtime
{
    public class HealthState
    {
        private bool _inHospital;
        private bool _intensiveCare;
        private DateTime _deadDate;
        private bool trigger;
        private bool _isDead;

        public HealthState()
        {


        }
        /// <summary>
        /// Checks if the person has symptoms and is alive. If these requirements are met, the health state will be updated. 
        /// The pre-illness is taken into account in the health state update.
        /// </summary>
        /// <param name="person">Called person in the simulation</param>
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
        }

       
    }
}
