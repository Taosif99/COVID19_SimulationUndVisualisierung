using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation.Runtime;
using Simulation.Edit;
using EditorObjects;

/// <summary>
/// Class which handles simulation specific properties and settings.
/// </summary>
public class SimulationMaster : MonoBehaviour
{


    [SerializeField] private EditorObjectsManager editorObjectsManager;


    public static SimulationMaster Instance;


    /// <summary>
    /// Dictionary which is used as global counter to count the infection sates.
    /// </summary>
    private Dictionary<Person.InfectionStates, int> _infectionStateCounter = new Dictionary<Person.InfectionStates, int>();



    private void Awake()
    {
        //if (Instance == null) Instance = this;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else if(Instance != null)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _infectionStateCounter.Add(Person.InfectionStates.Phase1, 0);
        _infectionStateCounter.Add(Person.InfectionStates.Phase2, 0);
        _infectionStateCounter.Add(Person.InfectionStates.Phase3, 0);
        _infectionStateCounter.Add(Person.InfectionStates.Phase4, 0);
        _infectionStateCounter.Add(Person.InfectionStates.Phase5, 0);

        //TODO rename uninfected to suspicipus or something
        //TODO GET ALL UNINFECRED
        int amountUninfected = CountAmountPeopleUninfected();
        _infectionStateCounter.Add(Person.InfectionStates.Uninfected, amountUninfected);
    }

    // Update is called once per frame
    void Update()
    {
        DebugPrintOfCounterValues();
    }

    /// <summary>
    /// Method which handles the counting of infection states if a state transition happens.
    /// </summary>
    /// <param name="infectionState">The new state after transition.</param>
    public void AddToGlobalCounter(Person.InfectionStates infectionState)
    {

        switch (infectionState)
        {
            case Person.InfectionStates.Phase1:
                _infectionStateCounter[Person.InfectionStates.Phase1] += 1;
                if(_infectionStateCounter[Person.InfectionStates.Uninfected] > 0)
                _infectionStateCounter[Person.InfectionStates.Uninfected] -= 1; //suceptible
                break;

            case Person.InfectionStates.Phase2:
                _infectionStateCounter[Person.InfectionStates.Phase2] += 1;
                break;

            case Person.InfectionStates.Phase3:
                _infectionStateCounter[Person.InfectionStates.Phase3] += 1;
                break;

            case Person.InfectionStates.Phase4:

                _infectionStateCounter[Person.InfectionStates.Phase1] -= 1;
                _infectionStateCounter[Person.InfectionStates.Phase2] -= 1;
                _infectionStateCounter[Person.InfectionStates.Phase3] -= 1;
                _infectionStateCounter[Person.InfectionStates.Phase4] += 1;
                break;


            case Person.InfectionStates.Phase5:
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
        _infectionStateCounter[Person.InfectionStates.Uninfected] = CountAmountPeopleUninfected();
    }

    //TODO MAKE EFFICIENT IN EDITOR OBJECTS MANAGER
    private int CountAmountPeopleUninfected()
    {
        List<IEditorObject> editorObjects = editorObjectsManager.GetAllEditorObjects();
        int amountPeople = 0;

        //TODO BETTER COUNTING, probably while creating editor objects
        foreach (IEditorObject editorObject in editorObjects) 
        {
            Simulation.Edit.Entity entity = editorObject.EditorEntity;
            if (entity is Simulation.Edit.Household) 
            {
                Simulation.Edit.Household household = (Simulation.Edit.Household)entity;
                amountPeople += household.NumberOfPeople;
            }
        }
        return amountPeople;
    }


    //TODO PROPERTIES
    public int GetAmountInfected()
    {
        return _infectionStateCounter[Person.InfectionStates.Phase1];
    }

    public int GetAmountUninfected()
    {
        return _infectionStateCounter[Person.InfectionStates.Uninfected];
    }

    public int GetAmountRecovered()
    {

        return _infectionStateCounter[Person.InfectionStates.Phase5];

    }



    private void DebugPrintOfCounterValues()
    {

        string debugText = $"<color=yellow>Amount Infected/Phase 1: {   _infectionStateCounter[Person.InfectionStates.Phase1] } </color>" +
            $"<color=red> Amount phase 2:{   _infectionStateCounter[Person.InfectionStates.Phase2] } </color>" +
            $"<color=brown> Amount phase 3:{   _infectionStateCounter[Person.InfectionStates.Phase3] } </color>" +
            $"<color=chocolate>Amount phase 4:{   _infectionStateCounter[Person.InfectionStates.Phase4] } </color>" +
            $"<color=chartreuse>Amount phase 5:{   _infectionStateCounter[Person.InfectionStates.Phase5] }</color>" +
            $"<color=chartreuse>Amount uninfected:{_infectionStateCounter[Person.InfectionStates.Uninfected] }</color>";
        Debug.Log(debugText);
    
    }


}
