using System;
using System.Collections.Generic;

namespace Simulation.Runtime
{
    // TODO: Separate statistical fields
    class Person
    {
        private Venue _currentLocation;

        private InfectionStates _infectionStates;
        private PhysicalCondition _physicalCondition;
        private float _carefulnessFactor;
        private float _risk;
        private int _encounters;
        private int _amountOfPeopleInfected;
        private bool _isVaccinated;
        private DateTime _infectionDate;

        public Person(float carefulnessFactor, float risk, bool isWorker)
        {
            _carefulnessFactor = carefulnessFactor;
            _risk = risk;
            IsWorker = isWorker;
        }

        public bool IsWorker { get; }
        public List<Activity> Activities { get; } = new List<Activity>();

        public Venue CurrentLocation
        {
            get => _currentLocation;
            set
            {
                if (_currentLocation == value)
                {
                    return;
                }

                value?.MovePersonHere(this);
                _currentLocation = value;
            }
        }

        [Flags]
        enum InfectionStates
        {
            Uninfected,
            Infected,
            Infectious,
            Symptoms,
            Recovering,
            Recovered
        }

        enum PhysicalCondition
        {
            Healthy,
            Sick,
            VerySick
        }

        public void UpdateInfectionState()
        { 
        
        //TODO REZA UND TAOSIF
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
                    int timeInMinutes = dateTime.Hour * 60 + dateTime.Minute;
                    if (activity.StartTime <= timeInMinutes && activity.EndTime > timeInMinutes)
                    {
                        return activity;
                    }
                }
            }

            return null;
        }
    }
}
