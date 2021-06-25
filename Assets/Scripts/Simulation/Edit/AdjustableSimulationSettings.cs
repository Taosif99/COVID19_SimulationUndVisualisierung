using System;
using UnityEngine;

namespace Simulation.Edit
{
    /// <summary>
    /// Class which encapsulates the adjustable Simulation settings.
    /// </summary>
    [Serializable]
    public class AdjustableSimulationSettings
    {

        //Infection states, using floats better ???

        public int LatencyTime { get; set; }

        public int AmountDaysInfectious { get; set; }

        public  int EndDayInfectious  {get { return LatencyTime + AmountDaysInfectious - 1; } }

        public int IncubationTime { get; set; }
        public  int AmountDaysSymptoms { get; set; }

        public int EndDaySymptoms { get { return IncubationTime + AmountDaysSymptoms - 1; } } 



        //Default constructor loads values from static class
        public AdjustableSimulationSettings()
        {

            LatencyTime = DefaultInfectionParameters.InfectionsPhaseParameters.LatencyTime;
            AmountDaysInfectious = DefaultInfectionParameters.InfectionsPhaseParameters.AmountDaysInfectious;
            IncubationTime = DefaultInfectionParameters.InfectionsPhaseParameters.IncubationTime;
            AmountDaysSymptoms = DefaultInfectionParameters.InfectionsPhaseParameters.AmountDaysSymptoms;

        }


    }
}