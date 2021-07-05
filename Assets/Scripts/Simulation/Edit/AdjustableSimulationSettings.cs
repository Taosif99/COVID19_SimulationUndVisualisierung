using System;


namespace Simulation.Edit
{
    /// <summary>
    /// Class which encapsulates the serialized adjustable simulation settings.
    /// 
    /// CONSIDER: Hospital health parameter are stil dependent on regular health parameters (because of incubation time)
    /// </summary>
    [Serializable]
    public class AdjustableSimulationSettings
    {

        //Quarantine parameters
        public int AmountDaysQuarantine { get; set; }
        public int AdvancedQuarantineDays { get; set; }

        //Infection phase parameters
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

        public int DeathDay { get { return IncubationTime + DaysFromSymptomsBeginToDeath - 1; } }

        //Hospital health parameters
        public int DaysInHospital
        {
            get; set;
        }

        public int DurationOfSymtombeginToHospitalization { get; set; }
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
        public int DaysInIntensiveCare { get; set; }

        public int DurationOfHospitalizationToIntensiveCare { get; set; }
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

            LatencyTime = DefaultInfectionParameters.InfectionPhaseParameters.LatencyTime;
            AmountDaysInfectious = DefaultInfectionParameters.InfectionPhaseParameters.AmountDaysInfectious;
            IncubationTime = DefaultInfectionParameters.InfectionPhaseParameters.IncubationTime;
            AmountDaysSymptoms = DefaultInfectionParameters.InfectionPhaseParameters.AmountDaysSymptoms;

            RecoveringProbability = DefaultInfectionParameters.HealthPhaseParameters.RecoveringProbability;
            RecoveringInHospitalProbability = DefaultInfectionParameters.HealthPhaseParameters.RecoveringInHospitalProbability;
            PersonSurvivesIntensiveCareProbability = DefaultInfectionParameters.HealthPhaseParameters.PersonSurvivesIntensiveCareProbability;
            DaysFromSymptomsBeginToDeath = DefaultInfectionParameters.HealthPhaseParameters.DaysFromSymptomsBeginToDeath;

            DaysInHospital = DefaultInfectionParameters.HealthPhaseParameters.DaysInHospital;
            DurationOfSymtombeginToHospitalization = DefaultInfectionParameters.HealthPhaseParameters.DurationOfSymtombeginToHospitalization;
            DaysInIntensiveCare = DefaultInfectionParameters.HealthPhaseParameters.DaysInIntensiveCare;
            DurationOfHospitalizationToIntensiveCare = DefaultInfectionParameters.HealthPhaseParameters.DurationOfHospitalizationToIntensiveCare;

            AmountDaysQuarantine = DefaultInfectionParameters.QuarantineParameters.QuarantineDays;
            AdvancedQuarantineDays = DefaultInfectionParameters.QuarantineParameters.AdvancedQuarantineDays;
        }

        /// <summary>
        /// Methods which makes sure that: 
        /// - The adjustesd end day of symptoms is always greater or equal
        /// the last infectious day.
        /// - A Person only can go to a hospital if he/she has symptoms
        /// - That intensive care is a "subset" of hospitalization !
        /// </summary>
        /// <returns>true if ranges are valid, else false</returns>
        public bool RangesAreValid()
        {

            bool validSimulationPhases = EndDaySymptoms >= EndDayInfectious;
            bool validHealthPhaseParameters = DayAPersonMustGoToHospital < EndDaySymptoms
                                            && DayAPersonMustGoToHospital >= IncubationTime;
            bool validHealthPhaseHospitalParameters = DayAPersonMustGoToIntensiveCare >= DayAPersonMustGoToHospital
                                                     && DayAPersonCanLeaveIntensiveCare <= DayAPersonCanLeaveTheHospital;
            return validSimulationPhases && validHealthPhaseParameters && validHealthPhaseHospitalParameters;
        }
    }
}