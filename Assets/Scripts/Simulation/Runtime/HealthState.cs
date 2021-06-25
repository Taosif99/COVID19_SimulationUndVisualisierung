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

        /// <summary>
        /// On the basis of given parameters it is determined whether the person is pre-diseased.
        /// </summary>
        void SetHealthCondition()
        {
            float healthyProbability = Random.Range(0f, 1f);
            Simulation.Edit.AdjustableSimulationSettings settings = SimulationMaster.Instance.AdjustableSettings;
            
            /*
            if (healthyProbability <= settings.PreIllnessProbability)
            {
                _preIllness = true;
            }*/
        }

        /// <summary>
        /// A random survive probaility will be calculated
        /// </summary>
        public void SurviveProbability()
        {
            float _surviveProbability = Random.Range(0f, 1f);
        }

        /// <summary>
        /// Checks if the person has symptoms and is alive. If these requirements are met, the health state will be updated. 
        /// The pre-illness is taken into account in the health state update.
        /// </summary>
        /// <param name="person">Called person in the simulation</param>
        public void UpdateHealthState(Person person)
        {
            if (person.InfectionState.HasFlag(Person.InfectionStates.Symptoms) & _isDead == false)
            {
                Simulation.Edit.AdjustableSimulationSettings settings = SimulationMaster.Instance.AdjustableSettings;


                /*
                if (_preIllness == false)
                {
                    if (_surviveProbability <= settings.FatalityRate)
                        _isDead = true;
                }
                else
                {
                    if (_surviveProbability <= settings.FatalityRatePreIllness)
                        _isDead = true;
                }*/
            }
        }
    }
}
