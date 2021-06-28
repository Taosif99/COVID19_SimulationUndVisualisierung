using System;
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
        //private int _encounters;
        //private int _amountOfPeopleInfected;
        //private bool _isVaccinated;
        private DateTime _infectionDate;
        //private int _infectionStateDuration;
        private HealthState _healthState;

        private bool _isDead = false;
        private double _daysSinceInfection;

        public Person(float carefulnessFactor, float risk, bool isWorker)
        {
            CarefulnessFactor = carefulnessFactor;
            _risk = risk;
            IsWorker = isWorker;
            _healthState = new HealthState(this);
        }

        public float CarefulnessFactor { get; }
        public InfectionStates InfectionState { get; private set; }
        public bool IsWorker { get; }
        public List<Activity> Activities { get; } = new List<Activity>();
        public Venue CurrentLocation { get; set; }
        public bool IsDead { get => _isDead; set => _isDead = value; }
        public double DaysSinceInfection { get => _daysSinceInfection; set => _daysSinceInfection = value; }

        public event Action<StateTransitionEventArgs> OnStateTrasitionHandler;
        public class StateTransitionEventArgs : EventArgs
        {
            public InfectionStates newInfectionState;
            public InfectionStates previousInfectionState;
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

        public void UpdateHealthState()
        {
            _healthState.UpdateHealthState();
        }



        /// <summary>
        /// Calculates the difference between the current date and the infection date in days.
        /// The difference determines in which infection states the person is. 
        /// The infection status is stored in the attribute public InfectionStates _infectionStates.
        /// </summary>
        /// <param name="currentDate">Current simulations date</param>

        public void UpdateInfectionState(DateTime currentDate)
        {
        

           

            if (!_infectionDate.Equals(new DateTime())) //Without this all persons will be "recovered"
            {
                int currentDay = currentDate.Day;
                int currentMonth = currentDate.Month;
                _daysSinceInfection = (currentDate - _infectionDate).TotalDays;
                bool stateTransition = false;
                Simulation.Edit.AdjustableSimulationSettings settings = SimulationMaster.Instance.AdjustableSettings;
                InfectionStates previousState = InfectionStates.Uninfected;


         
                

                switch (InfectionState)
                {

                    case InfectionStates.Phase1:
                        {
                            //_healthState.UpdateHealthState(currentDate, _infectionDate);
                            if (_daysSinceInfection >= settings.LatencyTime
                                && _daysSinceInfection <= settings.EndDayInfectious)
                            {
                                stateTransition = true;
                                InfectionState = InfectionStates.Phase2;
                                previousState = InfectionStates.Phase1;
                            }

                            break;
                        }

                    case InfectionStates.Phase2:
                        {
                            //_healthState.UpdateHealthState(currentDate, _infectionDate);
                            if (_daysSinceInfection >= settings.IncubationTime
                                && _daysSinceInfection <= settings.EndDaySymptoms
                                && _daysSinceInfection <= settings.EndDayInfectious)
                            {
                                stateTransition = true;
                                InfectionState = InfectionStates.Phase3;
                                previousState = InfectionStates.Phase2;
                            }
                            break;
                        }
                    case InfectionStates.Phase3:
                        {

                            //If person will die no recovering possible
                            //TODO HANDLE PHASES OF DYING PERSONS
                            if (_healthState.WillDieInIntensiveCare) return;

                            // _healthState.UpdateHealthState(currentDate, _infectionDate);
                            if (_daysSinceInfection > settings.EndDayInfectious
                                && _daysSinceInfection <= settings.EndDaySymptoms)
                            {
                                stateTransition = true;
                                InfectionState = InfectionStates.Phase4;
                                previousState = InfectionStates.Phase3;
                            }

                            
                            //Special case if EndDayInfectious == EndDaySymptoms
                            if (settings.EndDayInfectious == settings.EndDaySymptoms 
                                &&_daysSinceInfection > settings.EndDayInfectious
                                && _daysSinceInfection > settings.EndDaySymptoms)
                            {
                                stateTransition = true;
                                InfectionState = InfectionStates.Phase5;
                                previousState = InfectionStates.Phase3;
                            }
                            break;
                        }


                    case InfectionStates.Phase4:
                        {
                            //_healthState.UpdateHealthState(currentDate, _infectionDate);
                            if (_daysSinceInfection > settings.EndDaySymptoms)
                            {
                                stateTransition = true;
                                InfectionState = InfectionStates.Phase5;
                                previousState = InfectionStates.Phase4;

                                //Here we may update the infection risk if person recovers

                            }

                            break;
                        }

                    default:
                        break;
                }

                if (stateTransition)
                {
                    StateTransitionEventArgs stateTransitionEventArgs = new StateTransitionEventArgs();
                    stateTransitionEventArgs.newInfectionState = InfectionState;
                    stateTransitionEventArgs.previousInfectionState = previousState;
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
            InfectionStates previousState = InfectionState;
            InfectionState = InfectionStates.Infected;
            _infectionDate = infectionDate;
            //_infectionStateDuration = DefaultInfectionParameters2.InfectionsPhaseParameters.AmountDaysInfectious;
            StateTransitionEventArgs stateTransitionEventArgs = new StateTransitionEventArgs();
            stateTransitionEventArgs.newInfectionState = InfectionState;
            stateTransitionEventArgs.previousInfectionState = previousState;
            OnStateTrasitionHandler?.Invoke(stateTransitionEventArgs);
            SimulationMaster.Instance.OnPersonInfected();
        }

    }
}