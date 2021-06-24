using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation.Runtime;
using System;

namespace Simulation.Edit
{
    /// <summary>
    /// Class which encapsulates the adjustable Simulation settings.
    /// </summary>
    [Serializable]
    public class AdjustableSimulationSettings
    {

        //Infection states, using floats better ???
        public int IncubationMinDay { get; set; }
        public int IncubationMaxDay { get; set; }
        public int SymptomsMinDay { get; set; }
        public int SymptomsMaxDay { get; set; }
        public int InfectiousMinDay { get; set; }
        public int InfectiousMaxDay { get; set; }
        public int RecoveringMinDay { get; set; }
        public int RecoveringMaxDay { get; set; }
        //Death handling
        public float FatalityRate { get; set; }
        public float FatalityRatePreIllness { get; set; }
        public float PreIllnessProbability { get; set; }

        //Default constructor loads values from static class
        public AdjustableSimulationSettings()
        {
            /*
            IncubationMinDay = InfectionStateParameters.IncubationMinDay;
            IncubationMaxDay = InfectionStateParameters.IncubationMaxDay;
            SymptomsMinDay = InfectionStateParameters.SymptomsMinDay;
            SymptomsMaxDay = InfectionStateParameters.SymptomsMaxDay;
            InfectiousMinDay = InfectionStateParameters.InfectiousMinDay;
            InfectiousMaxDay = InfectionStateParameters.InfectiousMaxDay;
            RecoveringMinDay = InfectionStateParameters.RecoveringMinDay;
            RecoveringMaxDay = InfectionStateParameters.RecoveringMaxDay;
            FatalityRate = InfectionStateParameters.FatalityRate;
            FatalityRatePreIllness = InfectionStateParameters.FatalityRatePreIllness;
            PreIllnessProbability = InfectionStateParameters.PreIllnessProbability;
            */
            IncubationMinDay = DefaultInfectionParameters.IncubationMinDay;
            IncubationMaxDay = DefaultInfectionParameters.IncubationMaxDay;
            SymptomsMinDay = DefaultInfectionParameters.SymptomsMinDay;
            SymptomsMaxDay = DefaultInfectionParameters.SymptomsMaxDay;
            InfectiousMinDay = DefaultInfectionParameters.InfectiousMinDay;
            InfectiousMaxDay = DefaultInfectionParameters.InfectiousMaxDay;
            RecoveringMinDay = DefaultInfectionParameters.RecoveringMinDay;
            RecoveringMaxDay = DefaultInfectionParameters.RecoveringMaxDay;
            FatalityRate = DefaultInfectionParameters.FatalityRate;
            FatalityRatePreIllness = DefaultInfectionParameters.FatalityRatePreIllness;
            PreIllnessProbability = DefaultInfectionParameters.PreIllnessProbability;

            /*
            Debug.Log("Created simulation settings !");
            Debug.Log( IncubationMinDay + ", " +
                        IncubationMaxDay + ", " +
                        SymptomsMinDay + ", " +
                        SymptomsMaxDay + ", " +
                        InfectiousMinDay + ", " +
                        InfectiousMaxDay + ", " +
                        RecoveringMinDay + ", " +
                        RecoveringMaxDay + ", " +
                        FatalityRate + ", " +
                        FatalityRatePreIllness + ", " +
                        PreIllnessProbability + ", ");
            */
        }


    }
}