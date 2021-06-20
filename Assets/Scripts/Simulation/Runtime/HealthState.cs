using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Simulation.Runtime
{
    public class HealthState
    {
        private float _surviveProbability;
        private bool _preIllness;
        private bool _isDead;

        public HealthState()
        {
            SetHealthCondition();
        }

        void SetHealthCondition()
        {
            float healthyProbability = Random.Range(0f, 1f);
            if (healthyProbability <= InfectionStateParameters.preIllnessProbability)
            {
                _preIllness = true;
            }
        }

        public void SurviveProbability()
        {
            float _surviveProbability = Random.Range(0f, 1f);
        }

        public void UpdateHealthState(Person person)
        {
            if (person.InfectionState.HasFlag(Person.InfectionStates.Symptoms) & _isDead == false)
            {
                if (_preIllness == false)
                {
                    if (_surviveProbability <= InfectionStateParameters.FatalityRate)
                        _isDead = true;
                }
                else
                {
                    if (_surviveProbability <= InfectionStateParameters.FatalityRatePreIllness)
                        _isDead = true;
                }
            }
        }
    }
}
