using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class which encapsulates the adjustable Simulation settings.
/// </summary>
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
    public float preIllnessProbability { get; set; }

    //Default constructor loads values from static class
    public AdjustableSimulationSettings()
    { 
    
       
    
    
    
    
    
    }




}
