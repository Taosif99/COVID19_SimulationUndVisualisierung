using System;

namespace Simulation.Runtime
{
    // TODO: Separate statistical fields
    public class Person
    {
        private InfectionStates _infectionStates;
        private PhysicalCondition _physicalCondition;
        private float _carefulnessFactor;
        private float _risk;
        private int _encounters;
        private int _amountOfPeopleInfected;
        private bool _isVaccinated;
        private DateTime _infectionDate;
        private bool _isWorker;

        public Person(float carefulnessFactor, float risk, bool isWorker)
        {
            _carefulnessFactor = carefulnessFactor;
            _risk = risk;
            _isWorker = isWorker;
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

    }
}
