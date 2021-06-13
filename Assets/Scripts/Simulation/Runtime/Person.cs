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
        private bool _isDead;

        public Person(float carefulnessFactor, float risk, bool isWorker)
        {
            CarefulnessFactor = carefulnessFactor;
            _risk = risk;
            IsWorker = isWorker;
        }

        public float CarefulnessFactor { get; }
        public InfectionStates InfectionState { get; private set; }
        public bool IsWorker { get; }
        public List<Activity> Activities { get; } = new List<Activity>();
        public Venue CurrentLocation { get; set; }
        public bool IsDead { get; private set; }

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
            Phase4 = Symptoms | Recovering,
            Phase5 = Recovered

        }

        public enum PhysicalCondition
        {
            Healthy,
            PreIllness
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
                            _infectionStateDuration = Random.Range(InfectionStateDays.InfectiousMinDay, InfectionStateDays.InfectiousMaxDay);
                            stateTransition = true;
                        }

                        break;

                    case InfectionStates.Phase2:
                        if (daysSinceInfection > _infectionStateDuration)
                        {
                            InfectionState = InfectionStates.Phase3;
                            _infectionStateDuration = Random.Range(InfectionStateDays.SymptomsMinDay, InfectionStateDays.SymptomsMaxDay);
                            stateTransition = true;

                        }

                        break;

                    case InfectionStates.Phase3:
                        if (daysSinceInfection > _infectionStateDuration)
                        {
                            InfectionState = InfectionStates.Phase4;
                            _infectionStateDuration = Random.Range(InfectionStateDays.RecoveringMinDay, InfectionStateDays.RecoveringMaxDay);
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
                    SimulationMaster.Instance.AddToGlobalCounter(InfectionState);

                    //This causes problems with the object destruction on the graph -> update each day or each 12 hours---
                    //GlobalSimulationGraph.Instance.OnUpdate?.Invoke();
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

        /*
         * Get the physical Condition of a person.
         * 
        */
        public PhysicalCondition GetPhysicalCondition()
        {
            return _physicalCondition;
        }

        /*
         * Defines whether the person dies based on the physical state set.
         *  ,,Das Sterberisiko steigt bei den meisten Vorerkrankungen um bis zu 87 Prozent."
         *  Quelle: https://www.quarks.de/gesundheit/medizin/wie-viele-menschen-sterben-an-corona/ ,
         * 
         * ,,Insgesamt sind 2,6% aller Personen, für die bestätigte SARS-CoV-2-Infektionen in Deutschland übermittelt wurden, im Zusammenhang mit einer COVID-19-Erkrankung verstorben."
         * Quelle: 
         * https://www.rki.de/DE/Content/InfAZ/N/Neuartiges_Coronavirus/Steckbrief.html;jsessionid=9380EA03621C1ECA856E7B2B4D5A9E4A.internet062?nn=13490888#doc13776792bodyText13
         */
        public void UpdateHealthState()
        {
            if (InfectionState.HasFlag(InfectionStates.Symptoms) & _isDead == false)
            {
                float surviveProbability = Random.Range(0f, 1f);

                if (_physicalCondition.Equals(PhysicalCondition.Healthy))
                    if (surviveProbability <= 0.026f)
                        _isDead = true;
                    else
                    if (surviveProbability <= 0.87f)
                        _isDead = true;
            }
        }

        public void SetInfected(DateTime infectionDate)
        {
            InfectionState = InfectionStates.Infected;
            _infectionDate = infectionDate;
            _infectionStateDuration = Random.Range(InfectionStateDays.IncubationMinDay, InfectionStateDays.IncubationMaxDay);
        }
    }
}