using System.Collections.Generic;
using UnityEngine;
using Simulation.Runtime;
using EditorObjects;
using EpidemiologicalCalculation;
using System;

/// <summary>
/// Class which handles simulation global background simulation tasks
/// like global infection state counting and  passing calculations.
/// </summary>
public class SimulationMaster : MonoBehaviour
{
    [SerializeField] private EditorObjectsManager editorObjectsManager;

    public static SimulationMaster Instance;
    public Simulation.Edit.Simulation CurrentSimulation { get; set; }
    
    public bool IsForwardingSimulation { get; set; }

    /// <summary>
    /// The real world date the simulation started.
    /// </summary>
    public DateTime PlayDate { get; set; }

    private int _currentDayOfSimulation = 0;
    private DayInfoHandler _dayInfoHandler = new DayInfoHandler();
    private int _amountPeopleDead = 0;

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

    public int CurrentDayOfSimulation { get => _currentDayOfSimulation; private  set => _currentDayOfSimulation = value; }

    public Simulation.Edit.AdjustableSimulationSettings AdjustableSettings
    {
        get 
        { 
            return CurrentSimulation.SimulationOptions.AdjustableSimulationPrameters; 
        }
    }

    public int AmountPeopleDead { get => _amountPeopleDead; }
   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        IsForwardingSimulation = false;
        _infectionStateCounter.Add(Person.InfectionStates.Phase1, 0); 
        _infectionStateCounter.Add(Person.InfectionStates.Phase2, 0);
        _infectionStateCounter.Add(Person.InfectionStates.Phase3, 0);
        _infectionStateCounter.Add(Person.InfectionStates.Phase4, 0);
        _infectionStateCounter.Add(Person.InfectionStates.Phase5, 0); 
        _infectionStateCounter.Add(Person.InfectionStates.Uninfected, 0);
        _infectionStateCounter.Add(Person.InfectionStates.Infectious, 0);
        _amountPeopleDead = 0;
    }

    // Update is called once per frame
    void Update()
    {
       // DebugPrintOfCounterValues();
    }

    /// <summary>
    /// Method which handles the counting of infection states if a state transition happens.
    /// 
    /// TO CONSIDER : All phases are counted distinct, apart of phase 1 which is the amount of infected people.
    /// States like infectious and uninfected are also counted.
    /// </summary>
    /// <param name="infectionState">The new state after transition.</param>
    public void AddToGlobalCounter(Person.StateTransitionEventArgs eventArgs)
    {
        Person.InfectionStates newInfectionState = eventArgs.newInfectionState;
        Person.InfectionStates previousState = eventArgs.previousInfectionState;

        //Handle special case phase 5 -> phase 1 
        if (previousState == Person.InfectionStates.Phase5 && newInfectionState == Person.InfectionStates.Phase1)
        {
            _infectionStateCounter[Person.InfectionStates.Phase5] -= 1;
            _infectionStateCounter[Person.InfectionStates.Phase1] += 1;
            return;
        }

        //Handle special case phase 3 -> phase 5
        if (previousState == Person.InfectionStates.Phase3 && newInfectionState == Person.InfectionStates.Phase5)
        {
            _infectionStateCounter[Person.InfectionStates.Phase1] -= 1;
            _infectionStateCounter[Person.InfectionStates.Phase3] -= 1;
            _infectionStateCounter[Person.InfectionStates.Phase5] += 1;
            _infectionStateCounter[Person.InfectionStates.Infectious] -= 1;
            //symptoms-=1
            return;
        }

        //Handles regular phase change
        switch (newInfectionState)
        {
            case Person.InfectionStates.Phase1:
                _infectionStateCounter[Person.InfectionStates.Phase1] += 1;
                if(_infectionStateCounter[Person.InfectionStates.Uninfected] > 0 && previousState == Person.InfectionStates.Uninfected)
                _infectionStateCounter[Person.InfectionStates.Uninfected] -= 1; 
                break;

            case Person.InfectionStates.Phase2:
                _infectionStateCounter[Person.InfectionStates.Phase2] += 1;
                _infectionStateCounter[Person.InfectionStates.Infectious] += 1;
                break;

            case Person.InfectionStates.Phase3:
                _infectionStateCounter[Person.InfectionStates.Phase2] -= 1;
                _infectionStateCounter[Person.InfectionStates.Phase3] += 1;
                //symptoms+=1
                break;

            case Person.InfectionStates.Phase4:
                _infectionStateCounter[Person.InfectionStates.Phase3] -= 1;
                _infectionStateCounter[Person.InfectionStates.Infectious] -= 1;
                //symptoms-=1
                _infectionStateCounter[Person.InfectionStates.Phase4] += 1;
                break;

            case Person.InfectionStates.Phase5:
                _infectionStateCounter[Person.InfectionStates.Phase1] -= 1;
                _infectionStateCounter[Person.InfectionStates.Phase4] -= 1;
                _infectionStateCounter[Person.InfectionStates.Phase5] += 1;
                break;
        }
    }

    /// <summary>
    /// Method which resets the count of the states. 
    /// </summary>
    public void Reset()
    {
        _infectionStateCounter[Person.InfectionStates.Phase1] = 0;
        _infectionStateCounter[Person.InfectionStates.Phase2] = 0;
        _infectionStateCounter[Person.InfectionStates.Phase3] = 0;
        _infectionStateCounter[Person.InfectionStates.Phase4] = 0;
        _infectionStateCounter[Person.InfectionStates.Phase5] = 0;
        _infectionStateCounter[Person.InfectionStates.Infectious] = 0;
        _amountPeopleDead = 0;
        _dayInfoHandler = new DayInfoHandler();
        _currentDayOfSimulation = 0;


    }

    //Must be called before the simulation starts and after the file is loaded
    public void StartUninfectedCounting()
    {
        _infectionStateCounter[Person.InfectionStates.Uninfected] = editorObjectsManager.AmountPeople;
    }

    public int GetAmountAllPeople()
    {
        return editorObjectsManager.AmountPeople;
    }

    private void DebugPrintOfCounterValues()
    {
        /*
        string debugText = $"<color=yellow>Infected/Phase 1: {   _infectionStateCounter[Person.InfectionStates.Phase1] } </color>" +
            $"<color=red> Infected,Infectious/Phase 2:{   _infectionStateCounter[Person.InfectionStates.Phase2] } </color>" +
            $"<color=brown> Infected,Infectious,Symptoms/Phase 3:{   _infectionStateCounter[Person.InfectionStates.Phase3] } </color>" +
            $"<color=LightGreen>Infected,Symptoms,Recovering/Phase 4:{   _infectionStateCounter[Person.InfectionStates.Phase4] } </color>" +
            $"<color=green>Amount Recovered/Phase 5:{   _infectionStateCounter[Person.InfectionStates.Phase5] }</color>" +
            $"<color=gray>Amount uninfected:{_infectionStateCounter[Person.InfectionStates.Uninfected] }</color>";
        Debug.Log(debugText);
    */
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
       _dayInfoHandler.UpdateRValueAndIncidence(_currentDayOfSimulation,out rValue,out rValue7,out incidence,PlayDate);

        if (UIController.Instance.EpidemicInfoToggle.isOn)
        {
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
        _dayInfoHandler.AddNewInfectionToCurrentDate(_currentDayOfSimulation);
    }

    public void OnPersonDies()
    {
        _amountPeopleDead++;
        //Since the last infection phase of a dead Person is currently phase3 (TODO IMPROVE)
        _infectionStateCounter[Person.InfectionStates.Phase1] -= 1;
        _infectionStateCounter[Person.InfectionStates.Phase3] -= 1;
        _infectionStateCounter[Person.InfectionStates.Infectious] -= 1;

    }
}
