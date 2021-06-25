using System.Collections.Generic;
using UnityEngine;
using Simulation.Runtime;
using EditorObjects;
using EpidemiologicalCalculation;
using System;

/// <summary>
/// Class which handles simulation specific properties and settings.
/// </summary>
public class SimulationMaster : MonoBehaviour
{


    [SerializeField] private EditorObjectsManager editorObjectsManager;


    public static SimulationMaster Instance;
    public Simulation.Edit.Simulation CurrentSimulation { get; set; }
    
    /// <summary>
    /// The real world date the simulation started.
    /// </summary>
    public DateTime PlayDate { get; set; }


    private int _currentDayOfSimulation = 0;
    private DayInfoHandler _dayInfoHandler = new DayInfoHandler();
    


    /// <summary>
    /// Dictionary which is used as global counter to count the infection states.
    /// </summary>
    private Dictionary<Person.InfectionStates, int> _infectionStateCounter = new Dictionary<Person.InfectionStates, int>();

    public int AmountUninfected
    {
        get
        {
            return _infectionStateCounter[Person.InfectionStates.Uninfected];
        }
    }

    public int AmountInfected
    {
        get 
        {
          return  _infectionStateCounter[Person.InfectionStates.Phase1];
        }
    }


    public int AmountInfectious
    {
        get
        {
            return _infectionStateCounter[Person.InfectionStates.Infectious];
        }
    }


    public int AmountRecovered
    {
        get
        {
            return _infectionStateCounter[Person.InfectionStates.Phase5];
        }
    }

    public int CurrentDayOfSimulation { get => _currentDayOfSimulation; set => _currentDayOfSimulation = value; }


    public Simulation.Edit.AdjustableSimulationSettings AdjustableSettings
    {
        get 
        { 
            return CurrentSimulation.SimulationOptions.AdjustableSimulationPrameters; 
        }
        
    }


    private void Awake()
    {
        //if (Instance == null) Instance = this;
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _infectionStateCounter.Add(Person.InfectionStates.Phase1, 0); //Infected
        _infectionStateCounter.Add(Person.InfectionStates.Phase2, 0);
        _infectionStateCounter.Add(Person.InfectionStates.Phase3, 0);
        _infectionStateCounter.Add(Person.InfectionStates.Phase4, 0);
        _infectionStateCounter.Add(Person.InfectionStates.Phase5, 0); //Recovered

        //TODO rename uninfected to suspicipus or something
        //TODO GET ALL UNINFECRED
        _infectionStateCounter.Add(Person.InfectionStates.Uninfected, 0);
        _infectionStateCounter.Add(Person.InfectionStates.Infectious, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
        //DebugPrintOfCounterValues();
    }

    /// <summary>
    /// Method which handles the counting of infection states if a state transition happens.
    /// </summary>
    /// <param name="infectionState">The new state after transition.</param>
    public void AddToGlobalCounter(Person.StateTransitionEventArgs eventArgs)
    {
        Person.InfectionStates infectionState = eventArgs.newInfectionState;

        switch (infectionState)
        {
            case Person.InfectionStates.Phase1:
                _infectionStateCounter[Person.InfectionStates.Phase1] += 1;
                //Decreasing amount uninfected
                if(_infectionStateCounter[Person.InfectionStates.Uninfected] > 0)
                _infectionStateCounter[Person.InfectionStates.Uninfected] -= 1; //suceptible
                break;

            case Person.InfectionStates.Phase2:
                _infectionStateCounter[Person.InfectionStates.Phase2] += 1;
                _infectionStateCounter[Person.InfectionStates.Infectious] += 1;
                break;

            case Person.InfectionStates.Phase3:
                _infectionStateCounter[Person.InfectionStates.Phase3] += 1;
                break;

            case Person.InfectionStates.Phase4:

                //_infectionStateCounter[Person.InfectionStates.Phase1] -= 1;
                _infectionStateCounter[Person.InfectionStates.Phase2] -= 1;
                _infectionStateCounter[Person.InfectionStates.Phase3] -= 1;
                _infectionStateCounter[Person.InfectionStates.Infectious] -= 1;
                _infectionStateCounter[Person.InfectionStates.Phase4] += 1;
                break;


            case Person.InfectionStates.Phase5:
                _infectionStateCounter[Person.InfectionStates.Phase1] -= 1; //added
                _infectionStateCounter[Person.InfectionStates.Phase4] -= 1;
                _infectionStateCounter[Person.InfectionStates.Phase5] += 1;
                break;
        }

    }

    /// <summary>
    /// Method which resets the count of the states. //TODO CALL
    /// </summary>
    public void Reset()
    {
        _infectionStateCounter[Person.InfectionStates.Phase1] = 0;
        _infectionStateCounter[Person.InfectionStates.Phase2] = 0;
        _infectionStateCounter[Person.InfectionStates.Phase3] = 0;
        _infectionStateCounter[Person.InfectionStates.Phase4] = 0;
        _infectionStateCounter[Person.InfectionStates.Phase5] = 0;
        //_infectionStateCounter[Person.InfectionStates.Uninfected] = ;
        _dayInfoHandler = new DayInfoHandler();
        _currentDayOfSimulation = 0;

    }

    //Must be called before the simulation starts and after the file is loaded
    public void StartUninfectedCounting()
    {
        _infectionStateCounter[Person.InfectionStates.Uninfected] = editorObjectsManager.AmountPeople;
    }


    private void DebugPrintOfCounterValues()
    {

        string debugText = $"<color=yellow>Infected/Phase 1: {   _infectionStateCounter[Person.InfectionStates.Phase1] } </color>" +
            $"<color=red> Infected,Infectious/Phase 2:{   _infectionStateCounter[Person.InfectionStates.Phase2] } </color>" +
            $"<color=brown> Infected,Infectious,Symptoms/Phase 3:{   _infectionStateCounter[Person.InfectionStates.Phase3] } </color>" +
            $"<color=LightGreen>Infected,Symptoms,Recovering/Phase 4:{   _infectionStateCounter[Person.InfectionStates.Phase4] } </color>" +
            $"<color=green>Amount Recovered/Phase 5:{   _infectionStateCounter[Person.InfectionStates.Phase5] }</color>" +
            $"<color=gray>Amount uninfected:{_infectionStateCounter[Person.InfectionStates.Uninfected] }</color>";
        Debug.Log(debugText);
    
    }




    //wrapping also DayInfoHandler
    public void OnDayBegins(DateTime date)
    {
        _currentDayOfSimulation += 1;
        _dayInfoHandler.AddDayInfo(date);
    }

    public void OnDayEnds()
    {
        float rValue;
        float rValue7;
        float incidence;
       _dayInfoHandler.UpdateRValueAndIncidence(CurrentDayOfSimulation,out rValue,out rValue7,out incidence,PlayDate);


        if (UIController.Instance.EpidemicInfoToggle.isOn)
        {

            //We may set the R-Value here
            if (rValue == -1f)
                UIController.Instance.RValueText.text = "R-Value: ? ";
            else
            {
                UIController.Instance.RValueText.text = $"R-Value: " + rValue.ToString("0.00");
            }

            if (rValue7 == -1f)
            {
                UIController.Instance.RValue7Text.text = "R-Value7: ? ";

            }
            else
            {
                UIController.Instance.RValue7Text.text = "R-Value7: " + rValue7.ToString("0.00");
            }

            if (incidence == -1f)
            {
                UIController.Instance.IncidenceText.text = "Incidence: ? ";
            }
            else
            {
                UIController.Instance.IncidenceText.text = "Incidence: "+ incidence.ToString("0.00");
            }
        }
    }



    public void OnPersonInfected()
    {
        _dayInfoHandler.AddNewInfectionToCurrentDate(CurrentDayOfSimulation);
    }

}
