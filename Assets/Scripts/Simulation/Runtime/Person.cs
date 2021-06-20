﻿using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Simulation.Runtime
{
    // TODO: Separate statistical fields
    public class Person
    {
        //private PhysicalCondition _physicalCondition;
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

            if (!_infectionDate.Equals(new DateTime())) //Without this all persons will be "recovered"
            {

                switch (InfectionState)
                {
                    case InfectionStates.Phase1:
                        if (daysSinceInfection > _infectionStateDuration)
                        {
                            InfectionState = InfectionStates.Phase2;
                            _infectionStateDuration = Random.Range(InfectionStateParameters.InfectiousMinDay, InfectionStateParameters.InfectiousMaxDay);
                            stateTransition = true;
                        }
                       
                        break;

                    case InfectionStates.Phase2:
                        if (daysSinceInfection > _infectionStateDuration)
                        {
                            InfectionState = InfectionStates.Phase3;
                            _infectionStateDuration = Random.Range(InfectionStateParameters.SymptomsMinDay, InfectionStateParameters.SymptomsMaxDay);
                            stateTransition = true;
                        }

                        break;

                    case InfectionStates.Phase3:
                        if (daysSinceInfection > _infectionStateDuration)
                        {
                            InfectionState = InfectionStates.Phase4;
                            _infectionStateDuration = Random.Range(InfectionStateParameters.RecoveringMinDay, InfectionStateParameters.RecoveringMaxDay);
                            stateTransition = true;

                        }

                        break;

                    case InfectionStates.Phase4:

                        if (daysSinceInfection > _infectionStateDuration)
                        {
                            InfectionState = InfectionStates.Phase5;
                            _infectionStateDuration = int.MaxValue;
                            stateTransition = true;

                        }

                        break;

                    case InfectionStates.Phase5:
                        break;
                }

                if (stateTransition)
                {
                    StateTransitionEventArgs stateTransitionEventArgs = new StateTransitionEventArgs();
                    stateTransitionEventArgs.newInfectionState = InfectionState;
                    OnStateTrasitionHandler?.Invoke(stateTransitionEventArgs);
                }
                // Debug.Log("State: " + InfectionState);

            }
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

        public void SetInfected(DateTime infectionDate)
        {
            InfectionState = InfectionStates.Infected;
            _infectionDate = infectionDate;
            _infectionStateDuration = Random.Range(InfectionStateParameters.IncubationMinDay, InfectionStateParameters.IncubationMaxDay);
            StateTransitionEventArgs stateTransitionEventArgs = new StateTransitionEventArgs();
            stateTransitionEventArgs.newInfectionState = InfectionState;
            OnStateTrasitionHandler?.Invoke(stateTransitionEventArgs);
            SimulationMaster.Instance.OnPersonInfected();
            _healthState.SurviveProbability();
        }
    }
}