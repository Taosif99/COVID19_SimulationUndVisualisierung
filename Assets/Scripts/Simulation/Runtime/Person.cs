using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulation.Runtime
{
    // TODO: Separate statistical fields
    public class Person
    {
        
        //private int _encounters;
        //private int _amountOfPeopleInfected;
        //private bool _isVaccinated;
        private HealthState _healthState;

        private bool _isInQuarantine = false;
        private DateTime _endDateOfQuarantine;

        public Person(float carefulnessFactor, bool isWorker)
        {
            CarefulnessFactor = carefulnessFactor;
            InfectionRisk = DefaultInfectionParameters.ProbabilityOfInfection;
            IsWorker = isWorker;
            _healthState = new HealthState(this);
        }

        public float CarefulnessFactor { get; }
        public InfectionStates InfectionState { get;  set; }
        public bool IsWorker { get; }
        public List<Activity> Activities { get; } = new List<Activity>();
        public Venue CurrentLocation { get; set; }

        public bool IsDead { get; set; } = false;
        public double DaysSinceInfection { get; set; }

        public bool IsInHospitalization { get; set; } = false;
        public bool IsInIntensiveCare { get; set; } = false;
        public bool HasRegularBed { get; set; } = false;
        public DateTime InfectionDate { get; set; }
        public float InfectionRisk { get; set; }

        public bool IsInQuarantine { get => _isInQuarantine; set => _isInQuarantine = value; }
        public DateTime EndDateOfQuarantine { get => _endDateOfQuarantine; set => _endDateOfQuarantine = value; }

        public event Action<StateTransitionEventArgs> OnStateTrasitionHandler;

        private Edit.AdjustableSimulationSettings _settings = SimulationMaster.Instance.AdjustableSettings;

        public class StateTransitionEventArgs : EventArgs
        {
            public InfectionStates newInfectionState;
            public InfectionStates previousInfectionState;
        }

        [Flags]
        public enum InfectionStates
        {
            Uninfected = 0, //susceptible
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

        public void SetReInfectionRisk()
        {
            InfectionRisk = _settings.InfectionRiskIfRecovered;
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
                if (activity.Days.HasFlag(dateTime.DayOfWeek.AsWeekDay()))
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
        /// Determine available time slots on the given <paramref name="dayOfWeek"/>,
        /// between the <paramref name="earliest"/> and <paramref name="latest"/> hour,
        /// based on the person's scheduled activities.
        /// </summary>
        /// 
        /// <param name="dayOfWeek">The day of the week for which available time slots should be determined</param>
        /// <param name="earliest">The earliest possible hour to be considered</param>
        /// <param name="latest">The latest hour which the time slots should not extend past</param>
        /// 
        /// <returns>
        /// A dictionary with the keys describing the start of the available time slot,
        /// and the values describing the length/duration of the time slot
        /// </returns>
        public Dictionary<int, int> DetermineAvailableTimeSlots(DayOfWeek dayOfWeek, int earliest, int latest)
        {
            // Key: time in hour, Value: duration/length in hours
            var availableTimeSlots = new Dictionary<int, int>();

            var activities = Activities
                .Where(a => a.Days.HasFlag(dayOfWeek.AsWeekDay()))
                .OrderBy(a => a.StartTime)
                .ToArray();

            if (activities.Length == 0)
            {
                availableTimeSlots.Add(earliest, latest - earliest);
                return availableTimeSlots;
            }
            
            // Determine free time slots before, after, or between activities
            for (var a = 0; a < activities.Length; a++)
            {
                Activity activity = activities[a];
                Activity followingActivity = activities.Length > a + 1
                    ? activities[a + 1]
                    : null;

                // This is the first activity of the day, and there is time before it begins
                if (a == 0 && activity.StartTime - earliest >= 1)
                {
                    availableTimeSlots.Add(earliest, activity.StartTime - earliest);
                }

                // This is the last activity of the day, and there is time after it ends
                if (followingActivity == null && latest - activity.EndTime >= 1)
                {
                    availableTimeSlots.Add(activity.EndTime, latest - activity.EndTime);
                }

                // There is an activity following this one, however there is time in-between
                if (followingActivity != null && followingActivity.StartTime - activity.EndTime >= 1)
                {
                    availableTimeSlots.Add(activity.EndTime, followingActivity.StartTime - activity.EndTime);
                }
            }

            return availableTimeSlots;
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
            //Without this all persons will be "recovered"
            if (InfectionDate == default)
            {
                return;
            }
            
            int currentDay = currentDate.Day;
            int currentMonth = currentDate.Month;
            DaysSinceInfection = (currentDate - InfectionDate).TotalDays;
            bool didTransitionState = false;
            Edit.AdjustableSimulationSettings settings = SimulationMaster.Instance.AdjustableSettings;
            InfectionStates previousState = InfectionState;

            //Hospital case is handled in healthState, until recovering they are in in phase 3
            if (IsInHospitalization)
            {
                return;
            }

            switch (InfectionState)
            {

                case InfectionStates.Phase1:
                {
                    if (DaysSinceInfection >= settings.LatencyTime
                        && DaysSinceInfection <= settings.EndDayInfectious)
                    {
                        didTransitionState = true;
                        InfectionState = InfectionStates.Phase2;
                    }

                    break;
                }

                case InfectionStates.Phase2:
                {
                    if (DaysSinceInfection >= settings.IncubationTime
                        && DaysSinceInfection <= settings.EndDaySymptoms
                        && DaysSinceInfection <= settings.EndDayInfectious)
                    {
                        didTransitionState = true;
                        InfectionState = InfectionStates.Phase3;
                    }
                            
                    break;
                }
                case InfectionStates.Phase3:
                {
                    //If person will die and no hospital is free
                    //person will die "regularly" in phase3 at home instead
                    //Here one may handle phases of dying
                    if (_healthState.WillDie)
                    {
                        return;
                    }
                            
                    if (DaysSinceInfection > settings.EndDayInfectious
                        && DaysSinceInfection <= settings.EndDaySymptoms)
                    {
                        didTransitionState = true;
                        InfectionState = InfectionStates.Phase4;
                    }
                            
                    //Special case if EndDayInfectious == EndDaySymptoms
                    if (settings.EndDayInfectious == settings.EndDaySymptoms 
                        &&DaysSinceInfection > settings.EndDayInfectious
                        && DaysSinceInfection > settings.EndDaySymptoms)
                    {
                        didTransitionState = true;
                        InfectionState = InfectionStates.Phase5;
                    }
                            
                    break;
                }


                case InfectionStates.Phase4:
                {
                    if (DaysSinceInfection > settings.EndDaySymptoms)
                    {
                        didTransitionState = true;
                        InfectionState = InfectionStates.Phase5;

                        //Here we may update the infection risk if person recovers
                        InfectionDate = default;//restore undefined infection date
                        SetReInfectionRisk();
                    
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

        /// <summary>
        /// Set the Infection state of a person to infected and set the current simulation date as the infection date of the person.
        ///  
        /// </summary>
        /// <param name="infectionDate">Current simulations date</param>
        public void SetInfected(DateTime infectionDate)
        {
            DaysSinceInfection = 0f;
            _healthState = new HealthState(this);
            InfectionStates previousState = InfectionState;
            InfectionState = InfectionStates.Infected;
            InfectionDate = infectionDate;
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
            return (!IsInIntensiveCare && _healthState.MustBeInIntensiveCare());
        }


        /// <summary>
        /// Method to check if a person can leave intensive care
        /// </summary>
        /// <returns>true if person can leave intensive care, else false</returns>
        public bool CanLeaveIntensiveCare()
        {

            return IsInIntensiveCare && !_healthState.MustBeInIntensiveCare();
        
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