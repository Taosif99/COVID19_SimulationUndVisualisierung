using System;
using System.Collections.Generic;


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
        private bool _isInHospitalization = false;
        private bool _isInIntensiveCare = false;
        private bool _hasRegularBed = false;
    
        private double _daysSinceInfection;

        public Person(float carefulnessFactor, float risk, bool isWorker)
        {
            CarefulnessFactor = carefulnessFactor;
            _risk = risk;
            IsWorker = isWorker;
            _healthState = new HealthState(this);
        }

        public float CarefulnessFactor { get; }
        public InfectionStates InfectionState { get;  set; }
        public bool IsWorker { get; }
        public List<Activity> Activities { get; } = new List<Activity>();
        public Venue CurrentLocation { get; set; }
        public bool IsDead { get => _isDead; set => _isDead = value; }
        public double DaysSinceInfection { get => _daysSinceInfection; set => _daysSinceInfection = value; }
        public bool IsInHospitalization { get => _isInHospitalization; set => _isInHospitalization = value; }
        public bool IsInIntensiveCare { get => _isInIntensiveCare; set => _isInIntensiveCare = value; }
        public bool HasRegularBed { get => _hasRegularBed; set => _hasRegularBed = value; }


        public event Action<StateTransitionEventArgs> OnStateTrasitionHandler;
        public class StateTransitionEventArgs : EventArgs
        {
            public InfectionStates newInfectionState;
            public InfectionStates previousInfectionState;
        }

        [Flags]
        public enum InfectionStates
        {
            Uninfected = 0, //susceptible TODO RENAME
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

        //Remove that ? if not used
        public enum PhysicalCondition
        {
            Healthy,
            PreIllness
        }

        public void SetReInfectionRisk()
        {
            _risk = Simulation.DefaultInfectionParameters.HealthPhaseParameters.InfectionRiskIfRecovered;
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
        /// Method which updates the current health state of a person.
        /// </summary>
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
                bool didTransitionState = false;
                Edit.AdjustableSimulationSettings settings = SimulationMaster.Instance.AdjustableSettings;
                InfectionStates previousState = InfectionState;

                if (IsInHospitalization) return; //Hospital case is handled in healthState

                switch (InfectionState)
                {

                    case InfectionStates.Phase1:
                        {
                            if (_daysSinceInfection >= settings.LatencyTime
                                && _daysSinceInfection <= settings.EndDayInfectious)
                            {
                                didTransitionState = true;
                                InfectionState = InfectionStates.Phase2;
                            }

                            break;
                        }

                    case InfectionStates.Phase2:
                        {
                            if (_daysSinceInfection >= settings.IncubationTime
                                && _daysSinceInfection <= settings.EndDaySymptoms
                                && _daysSinceInfection <= settings.EndDayInfectious)
                            {
                                didTransitionState = true;
                                InfectionState = InfectionStates.Phase3;
                            }
                            
                            break;
                        }
                    case InfectionStates.Phase3:
                        {
                            //If person will die and no hospital is free
                            //person will die "regularly" at home instead
                            //Here one may handle phases of dying
                            if (_healthState.WillDie)
                            {
                                return;
                            }
                            
                            if (_daysSinceInfection > settings.EndDayInfectious
                                && _daysSinceInfection <= settings.EndDaySymptoms)
                            {
                                didTransitionState = true;
                                InfectionState = InfectionStates.Phase4;
                            }
                            
                            //Special case if EndDayInfectious == EndDaySymptoms
                            if (settings.EndDayInfectious == settings.EndDaySymptoms 
                                &&_daysSinceInfection > settings.EndDayInfectious
                                && _daysSinceInfection > settings.EndDaySymptoms)
                            {
                                didTransitionState = true;
                                InfectionState = InfectionStates.Phase5;
                            }
                            
                            break;
                        }


                    case InfectionStates.Phase4:
                        {
                            if (_daysSinceInfection > settings.EndDaySymptoms)
                            {
                                didTransitionState = true;
                                InfectionState = InfectionStates.Phase5;

                                //Here we may update the infection risk if person recovers
                                _infectionDate = new DateTime(); //restore undefined infection date
                                SetReInfectionRisk();
                                _healthState = new HealthState(this);
                            }

                            break;
                        }
                }

                if (didTransitionState)
                {
                    OnStateTransition(InfectionState, previousState);
                    //Debug.Log($"Switching to {InfectionState}");
                }
            }
        }

        /// <summary>
        /// Set the Infection state of a person to infected and set the current simulation date as the infection date of the person.
        ///  
        /// </summary>
        /// <param name="infectionDate">Current simulations date</param>
        public void SetInfected(DateTime infectionDate)
        {
            _daysSinceInfection = 0f;
            InfectionStates previousState = InfectionState;
            InfectionState = InfectionStates.Infected;
            _infectionDate = infectionDate;
            OnStateTransition(InfectionStates.Infected, previousState);
            SimulationMaster.Instance.OnPersonInfected();
        }


        /// <summary>
        /// Method to check if a person must be transferred to a hospital
        /// </summary>
        /// <returns>true if person must be tranferred to a hospital, else false</returns>
        public bool MustBeTransferredToHospital()
        {
            return (!IsInHospitalization && _healthState.MustBeInHospital());

        }

        /// <summary>
        /// Method to check if a person must be transferred to intensive care
        /// </summary>
        /// <returns>true if person must be tranferred to intensive care, else false</returns>
        public bool MustBeTransferredToIntensiveCare()
        {
            return (!_isInIntensiveCare && _healthState.MustBeInIntensiveCare());
        }


        public bool CanLeaveIntensiveCare()
        {

            return _isInIntensiveCare && !_healthState.MustBeInIntensiveCare();
        
        }

        /// <summary>
        /// Method whic calls the state transition event of a person.
        /// </summary>
        /// <param name="newState"></param>
        /// <param name="previousState"></param>
        public void OnStateTransition(InfectionStates newState, InfectionStates previousState) 
        {
            StateTransitionEventArgs stateTransitionEventArgs = new StateTransitionEventArgs();
            stateTransitionEventArgs.newInfectionState = newState;
            stateTransitionEventArgs.previousInfectionState = previousState;
            OnStateTrasitionHandler?.Invoke(stateTransitionEventArgs);
        }

    }
}