using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Simulation.Runtime
{
    public class HealthState
    {
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
            Simulation.Edit.AdjustableSimulationSettings settings = SimulationMaster.Instance.AdjustableSettings;

            float probabilityToRecover = Random.Range(0f, 1f);
            if (probabilityToRecover <= settings.RecoveringProbability)
            {
                _willRecoverFromCoViD = true;
            }
            else
            {
                _willRecoverFromCoViD = false;
                float probabilityToRecoverInHospital = Random.Range(0f, 1f);

                if (probabilityToRecoverInHospital <= settings.RecoveringInHospitalProbability)
                {
                    _willRecoverInHosptal = true;
                }
                else
                {
                    _willRecoverInHosptal = false;
                    float probabilityToSurviveIntensiveCare = Random.Range(0f, 1f);
                    if (probabilityToSurviveIntensiveCare <= settings.PersonSurvivesIntensiveCareProbability)
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
                if (_person.DaysSinceInfection >= (settings.IncubationTime + settings.DaysFromSymptomsBeginToDeath - 1))
                {
                    _person.IsDead = true;
                    Debug.LogWarning("RIP");
                    SimulationMaster.Instance.OnPersonDies();
                }            
            }
        }
    }
}
