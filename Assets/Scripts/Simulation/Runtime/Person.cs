using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Simulation.Runtime
{
    // TODO: Separate statistical fields
    public class Person
    {
        private PhysicalCondition _physicalCondition;
        private float _risk;
        private int _encounters;
        private int _amountOfPeopleInfected;
        private bool _isVaccinated;
        private DateTime _infectionDate;
        private int _infectionStateDuration;
        private HealthState _healthState;

        public Person(float carefulnessFactor, float risk, bool isWorker)
        {
            CarefulnessFactor = carefulnessFactor;
            _risk = risk;
            IsWorker = isWorker;
            _healthState = new HealthState();
        }

        public float CarefulnessFactor { get; }
        public InfectionStates InfectionState { get; private set; }
        public bool IsWorker { get; }
        public List<Activity> Activities { get; } = new List<Activity>();
        public Venue CurrentLocation { get; set; }

        public event Action <StateTransitionEventArgs> OnStateTrasitionHandler;
        public class StateTransitionEventArgs : EventArgs
        {
            public InfectionStates newInfectionState;
        }


        [Flags]
        public enum InfectionStates
        {
            Uninfected = 0,
            Infected = 1,
            Infectious = 2,
            Symptoms = 4,
            Recovering = 8,
            Recovered = 16,

            Phase1 = Infected,
            Phase2 = Infected | Infectious,
            Phase3 = Infected | Infectious | Symptoms,
            Phase4 = Infected | Symptoms | Recovering,
            Phase5 = Recovered
        }

        public enum PhysicalCondition
        {
            Healthy,
            PreIllness
        }



        public bool HasActivityAt(DateTime dateTime) => GetActivityAt(dateTime) != null;

        public bool TryGetActivityAt(DateTime dateTime, out Activity activity)
        {
            return (activity = GetActivityAt(dateTime)) != null;
        }

        public Activity GetActivityAt(DateTime dateTime)
        {
            foreach (Activity activity in Activities)
            {
                if (activity.Days.HasFlag(dateTime.DayOfWeek.AsDayOfWeek()))
                {
                    float timeInHours = dateTime.Hour + (float)dateTime.Minute / 60;
                    if (activity.StartTime <= timeInHours && activity.EndTime > timeInHours)
                    {
                        return activity;
                    }
                }
            }

            return null;
        }



        /// <summary>
        /// Calculates the difference between the current date and the infection date in days.
        /// The difference determines in which infection states the person is. 
        /// The infection status is stored in the attribute public InfectionStates _infectionStates.
        /// </summary>
        /// <param name="currentDate">Current simulations date</param>

        public void UpdateInfectionState(DateTime currentDate)
        {
            int currentDay = currentDate.Day;
            int currentMonth = currentDate.Month;
            double daysSinceInfection = (currentDate - _infectionDate).TotalDays;
            bool stateTransition = false;

            Simulation.Edit.AdjustableSimulationSettings settings = SimulationMaster.Instance.AdjustableSettings;

            if (!_infectionDate.Equals(new DateTime())) //Without this all persons will be "recovered"
            {
                //Can be done better with if/else statements

                if (daysSinceInfection >= settings.LatencyTime
                    && daysSinceInfection  
                    <= (settings.EndDayInfectious) && InfectionState == InfectionStates.Phase1)
                {
                    stateTransition = true;
                    InfectionState = InfectionStates.Phase2;
                }
               
                if (daysSinceInfection >= settings.IncubationTime
                    && daysSinceInfection < settings.EndDaySymptoms && InfectionState == InfectionStates.Phase2)
                {
                    stateTransition = true;
                    InfectionState = InfectionStates.Phase3;
                }

                if (daysSinceInfection == settings.EndDaySymptoms && InfectionState == InfectionStates.Phase3)
                {
                    stateTransition = true;
                    InfectionState = InfectionStates.Phase4; 
                }

                if (daysSinceInfection > settings.EndDaySymptoms && InfectionState == InfectionStates.Phase4) 
                {
                    stateTransition = true;
                    InfectionState = InfectionStates.Phase5; 
                }

                if (stateTransition)
                {
                    StateTransitionEventArgs stateTransitionEventArgs = new StateTransitionEventArgs();
                    stateTransitionEventArgs.newInfectionState = InfectionState;
                    OnStateTrasitionHandler?.Invoke(stateTransitionEventArgs);
                    
                    Debug.Log($"Switching to {InfectionState}");
                }

            }
        }

        /// <summary>
        /// Set the Infection state of a person to infected and set the current simulation date as the infection date of the person.
        /// In addition, an incubation time and the survive probability of the person are set. 
        /// </summary>
        /// <param name="infectionDate">Current simulations date</param>
        public void SetInfected(DateTime infectionDate)
        {
            //Simulation.Edit.AdjustableSimulationSettings settings = SimulationMaster.Instance.AdjustableSettings;
            InfectionState = InfectionStates.Infected;
            _infectionDate = infectionDate;
            //_infectionStateDuration = DefaultInfectionParameters2.InfectionsPhaseParameters.AmountDaysInfectious;
            StateTransitionEventArgs stateTransitionEventArgs = new StateTransitionEventArgs();
            stateTransitionEventArgs.newInfectionState = InfectionState;
            OnStateTrasitionHandler?.Invoke(stateTransitionEventArgs);
            SimulationMaster.Instance.OnPersonInfected();
        }

    }
}