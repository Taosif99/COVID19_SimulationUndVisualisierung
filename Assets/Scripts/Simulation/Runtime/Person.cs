using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Simulation.Runtime
{
    // TODO: Separate statistical fields
    class Person
    {
        private Venue _currentLocation;

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

        public InfectionStates InfectionState { get; private set; }
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

                _currentLocation = value;
                value?.MovePersonHere(this);
            }
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

            Phase_0 = Uninfected,
            Phase_1 = Infected | Infectious,
            Phase_2 = Infected | Infectious | Symptoms,
            Phase_3 = Symptoms | Recovering,
            Phase_4 = Recovered

        }

        enum PhysicalCondition
        {
            Healthy,
            Sick,
            VerySick
        }

        /* 
         * Calculates the difference between the current date and the infection date in days.
         * The difference determines in which infection states the person is. 
         * The infection status is stored in the attribute public InfectionStates _infectionStates.
         *
         */

        public void UpdateInfectionState(DateTime currentDate)
        {
            int currentDay = currentDate.Day;
            int currentMonth = currentDate.Month;
            double _daysSinceInfection = _daysSinceInfection = (currentDate - _infectionDate).TotalDays;
            //Debug.Log(_daysSinceInfection);

            switch (InfectionState)
            {
                case InfectionStates.Phase_0:
                    break;
                
                case InfectionStates.Infected:
                    int InfectedDay = UnityEngine.Random.Range(InfectionStateDays.IncubationMinDay, InfectionStateDays.IncubationMaxDay);
                    if (_daysSinceInfection <= InfectedDay)
                    {
                        InfectionState = InfectionStates.Phase_0;
                    }
                    else
                    {
                        InfectionState = InfectionStates.Phase_1;
                    }
                    
                    break;

                case InfectionStates.Phase_1:
                    int InfectiousDay = UnityEngine.Random.Range(InfectionStateDays.InfectiousMinDay, InfectionStateDays.InfectiousMaxDay);
                    if (_daysSinceInfection <= InfectiousDay)
                    {
                        InfectionState = InfectionStates.Phase_1;
                    }
                    else
                    {
                        InfectionState = InfectionStates.Phase_2;
                    }
                    
                    break;

                case InfectionStates.Phase_2:
                    int SymptomsDay = UnityEngine.Random.Range(InfectionStateDays.SymptomsMinDay, InfectionStateDays.SymptomsMaxDay);
                    if (_daysSinceInfection <= SymptomsDay)
                    {
                        InfectionState = InfectionStates.Phase_2;
                    }
                    else
                    {
                        InfectionState = InfectionStates.Phase_3;
                    }
                    
                    break;

                case InfectionStates.Phase_3:

                    int RecoveringDay = UnityEngine.Random.Range(InfectionStateDays.RecoveringMinDay, InfectionStateDays.RecoveringMaxDay);
                    if (_daysSinceInfection <= RecoveringDay)
                    {
                        InfectionState = InfectionStates.Phase_3;
                    }
                    else
                    {
                        InfectionState = InfectionStates.Phase_4;
                    }
                    
                    break;


                case InfectionStates.Phase_4:
                    break;
            }

            
            // Debug.Log("State: " + _infectionStates);

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
                    float timeInHours = dateTime.Hour + (float) dateTime.Minute / 60;
                    if (activity.StartTime <= timeInHours && activity.EndTime > timeInHours)
                    {
                        return activity;
                    }
                }
            }

            return null;
        }

        public void UpdateHealthState(DateTime simulationDate)
        {
            throw new NotImplementedException();
        }
    }
}
