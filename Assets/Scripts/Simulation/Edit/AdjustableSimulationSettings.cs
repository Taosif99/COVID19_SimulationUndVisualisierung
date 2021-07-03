using System;


namespace Simulation.Edit
{
    /// <summary>
    /// Class which encapsulates the adjustable simulation settings.
    /// 
    /// CONSIDER: Hospital health parameter are stil dependent on regular health parameters (because of incubation time)
    /// </summary>
    [Serializable]
    public class AdjustableSimulationSettings
    {
        //Quarantine parameters
        public int AmountDaysQuarantine { get; set; }

        //Infection states, using floats better ???
        public int LatencyTime { get; set; }

        public int AmountDaysInfectious { get; set; }

        public int EndDayInfectious { get { return LatencyTime + AmountDaysInfectious - 1; } }

        public int IncubationTime { get; set; }
        public int AmountDaysSymptoms { get; set; }

        public int EndDaySymptoms { get { return IncubationTime + AmountDaysSymptoms - 1; } }

        //regular health parameters
        public float RecoveringProbability { get; set; }
        public float RecoveringInHospitalProbability { get; set; }
        public float PersonSurvivesIntensiveCareProbability { get; set; }
        public int DaysFromSymptomsBeginToDeath { get; set; }


        //Hospital health parameter
        public int DaysInHospital
        {
            get
            {
                return DefaultInfectionParameters.HealthPhaseParameters.DaysInHospital;
            }
        }

        public int DurationOfSymtombeginToHospitalization
        {
            get
            {

                return DefaultInfectionParameters.HealthPhaseParameters.DurationOfSymtombeginToHospitalization;
            }

        }
        public int DayAPersonMustGoToHospital
        {
            get
            {
                return IncubationTime + DurationOfSymtombeginToHospitalization - 1;
            }
        }

        public int DayAPersonCanLeaveTheHospital
        {
            get
            {
                return DayAPersonMustGoToHospital + DaysInHospital;

            }
        }
        public int DaysInIntensiveCare
        {
            get
            {
                return DefaultInfectionParameters.HealthPhaseParameters.DaysInIntensiveCare;
            }
        }

        public int DurationOfHospitalizationToIntensiveCare
        {
            get
            {

                return DefaultInfectionParameters.HealthPhaseParameters.DurationOfHospitalizationToIntensiveCare;
            }
        }
        public int DayAPersonMustGoToIntensiveCare
        {
            get
            { 
                return DayAPersonMustGoToHospital + DurationOfHospitalizationToIntensiveCare; 
            }
        }
        public int DayAPersonCanLeaveIntensiveCare
        {
            get
            {
                return DayAPersonMustGoToIntensiveCare + DaysInIntensiveCare;

            }
        }

        //Default constructor loads values from static class
        public AdjustableSimulationSettings()
        {

            LatencyTime = DefaultInfectionParameters.InfectionsPhaseParameters.LatencyTime;
            AmountDaysInfectious = DefaultInfectionParameters.InfectionsPhaseParameters.AmountDaysInfectious;
            IncubationTime = DefaultInfectionParameters.InfectionsPhaseParameters.IncubationTime;
            AmountDaysSymptoms = DefaultInfectionParameters.InfectionsPhaseParameters.AmountDaysSymptoms;

            RecoveringProbability = DefaultInfectionParameters.HealthPhaseParameters.RecoveringProbability;
            RecoveringInHospitalProbability = DefaultInfectionParameters.HealthPhaseParameters.RecoveringInHospitalProbability;
            PersonSurvivesIntensiveCareProbability = DefaultInfectionParameters.HealthPhaseParameters.PersonSurvivesIntensiveCareProbability;
            DaysFromSymptomsBeginToDeath = DefaultInfectionParameters.HealthPhaseParameters.DaysFromSymptomsBeginToDeath;

            AmountDaysQuarantine = DefaultInfectionParameters.QuarantineParameters.QuarantineDays;
        }
    }
}