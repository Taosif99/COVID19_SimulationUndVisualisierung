using System.Collections.Generic;
using UnityEngine;
using EditorObjects;
using Grid;
using UnityEngine.UI;
using Simulation.Edit;
using System.Linq;
using System;
using Simulation;
/// <summary>
/// This class maintains the editor Objects during the Runtime
/// I think it would be better to implement a "gameMaster" class to remove Monobehaviour inheritance
/// 
/// Currently working wick mock runtime objects
/// </summary>
public class EditorObjectsManager : MonoBehaviour
{

    public GridManager SimulationGridManager;


    public List<IEditorObject> editorObjects = new List<IEditorObject>();

    //This lists are currently NOT used
    /*
    public List<VenueEditorObject> venueEditorObjects = new List<VenueEditorObject>();
    public List<WorkplaceEditorObject> workplaceEditorObjects = new List<WorkplaceEditorObject>();
    public List<HospitalEditorObject> hospitalEditorObjects = new List<HospitalEditorObject>();
    public List<HouseholdEditorObject> householdEditorObjects = new List<HouseholdEditorObject>();
    */

    private Entity _currentSelectedEntity;



    ////////////////////////////////////////////////////////////////////////////////////////////////
    //The settings objects can be outsourced in an  Venue properties UI class
    //public Text ObjectNameText;
    //Venue elements
    public InputField ObjectNameInputField;
    public InputField InfectionRiskInputField;
    //public InputField CurrentPeopleAtVenueInputField; //Use Text or label later, remove from ui
    //Household elements
    public InputField NumberOfPeopleInputField;
    public InputField PercantageOfWorkersInputField;
    public InputField CarefulnessInputField;
    //Workplace elements
    public Dropdown WorkplaceTypeDropdown;
    public InputField CapacityInputField;
    //Hospital elements
    public Dropdown HospitalScaleDropdown;
    public Dropdown WorkerAvailabilityDropdown;
    //////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Method which adds an Editor object.
    /// </summary>
    /// <param name="namedPrefab"></param>
    /// <param name="spawnPosition"></param>
    /// <param name="relativePositionInt"></param>
    /// <param name="planeWorldTransform"></param>
    /// <returns></returns>
    public GameObject AddEditorObject(GridManager.NamedPrefab namedPrefab, Vector3 spawnPosition, Vector3Int relativePositionInt, Transform planeWorldTransform)
    {

        PrefabName currentPrefabName = namedPrefab.prefabName;
        GameObject currentPrefabToSpawn = namedPrefab.prefab;
        IEditorObject editorObject = null;


        switch (currentPrefabName)
        {

          /*  case PrefabName.Venue:
                editorObject = EditorObjectFactory.CreateVenueEditorObject(currentPrefabToSpawn, spawnPosition, relativePositionInt, planeWorldTransform);
                //venueEditorObjects.Add((VenueEditorObject)editorObject);
                break;*/

            case PrefabName.Workplace:
                editorObject = EditorObjectFactory.CreateWorkplaceEditorObject(currentPrefabToSpawn, spawnPosition, relativePositionInt, planeWorldTransform);
                //workplaceEditorObjects.Add((WorkplaceEditorObject)editorObject);
                break;

            case PrefabName.Hospital:
                editorObject = EditorObjectFactory.CreateHospitalEditorObject(currentPrefabToSpawn, spawnPosition, relativePositionInt, planeWorldTransform);
                //hospitalEditorObjects.Add((HospitalEditorObject)editorObject);
                break;

            case PrefabName.Household:
                editorObject = EditorObjectFactory.CreateHouseholdEditorObject(currentPrefabToSpawn, spawnPosition, relativePositionInt, planeWorldTransform);
                //householdEditorObjects.Add((HouseholdEditorObject)editorObject);
                break;

            default:
                Debug.LogError("Unknown prefab name");
                break;
        }

    

        editorObjects.Add(editorObject);
        //Load the new object in the ui, TODO MAKE OVERLOADED METHOD TO DIFFERETIATE BETWEEN UI CLICK WHERE SEARCH IS NEEDED AND THESE ONE
        LoadEditorObjectUI(spawnPosition);


        return editorObject.EditorGameObject;
    }


    //We may handle different venues according the properties what can be set
    
    /// <summary>
    /// Method which loads Values from the UI according the correspoding Clicked Venue Object Object
    /// </summary>
    /// <param name="spawnPosition"></param>
    public void LoadEditorObjectUI(Vector3 spawnPosition)
    {
        Debug.Log("Loading Object in position: " + spawnPosition);
        //Search the editor object --> better use dictionary and unique name
        //Searching objects by absolute position probably a better solution
        foreach (IEditorObject editorObject in editorObjects)
        {
            if (editorObject.EditorGameObject.transform.position == spawnPosition)
            {
                UIController.Instance.IsEntitySelectedUI(true);
                ObjectNameInputField.text = editorObject.UIName;
                Entity entity = editorObject.EditorEntity;
                if (entity != null) //Can be removed later
                {
                    _currentSelectedEntity = entity; //Must be set null when no object is clicked

                    //Check if Graph....TODO WHEN GRAPH IS UI ELEMENT IN WORLD
                    if (entity is Venue)
                    {
                        UIController.Instance.LoadVenueUI();
                        Venue venue = (Venue)entity;
                        InfectionRiskInputField.text = venue.InfectionRisk.ToString(); //TODO ROUND VALUES
                        if (entity is Workplace)
                        {
                            UIController.Instance.LoadWorkplaceUI();
                            Workplace workplace = (Workplace)entity;
                            List<string> availableWorkplaceOptions = WorkplaceTypeDropdown.options.Select(option => option.text).ToList();
                            WorkplaceTypeDropdown.value = availableWorkplaceOptions.IndexOf(workplace.Type.ToString());
                            CapacityInputField.text = workplace.WorkerCapacity.ToString();

                            if (entity is Hospital)
                            {
                                UIController.Instance.LoadHospitalUI();
                                //FIXME THAT DOES NOT WORK
                                Hospital hospital = (Hospital)entity;
                                List<string> availableHospitalScaleOptions = HospitalScaleDropdown.options.Select(option => option.text).ToList();
                                List<string> availableWorkerAvailabilityOptions = WorkerAvailabilityDropdown.options.Select(option => option.text).ToList();
                                HospitalScaleDropdown.value = availableHospitalScaleOptions.IndexOf(hospital.Scale.ToString());
                                WorkerAvailabilityDropdown.value = availableWorkerAvailabilityOptions.IndexOf(hospital.WorkerAvailability.ToString());
                            }

                        }
                        else if (entity is Household)
                        {
                            UIController.Instance.LoadHouseholdUI();
                            Household household = (Household)entity;
                            //I need the saved Objects for that...
                            NumberOfPeopleInputField.text = household.NumberOfPeople.ToString();
                            CarefulnessInputField.text = household.CarefulnessTendency.ToString();
                            PercantageOfWorkersInputField.text = household.PercentageOfWorkers.ToString();
                        }
                    }

                    UIController.Instance.UnselectSelectedButton();
                }
                return;
            }
        }

    }

    //TODO CATCH INPUT ERRORS

    /// <summary>
    /// 
    /// Method which saves changes (currently) to runtime
    /// objects and Editor objects
    /// 
    /// TODO Load save values to runtime entity
    /// Enum parsing looks ugly
    /// </summary>
    public void SaveToEntity()
    {
        if (_currentSelectedEntity != null)
        {

            //Get the edito object to save the UI name
            foreach (IEditorObject editorObject in editorObjects)
            {
                if (editorObject.EditorEntity== _currentSelectedEntity) 
                {
                    editorObject.UIName = ObjectNameInputField.text;
                    break; 
                }
           }
            if (_currentSelectedEntity is Venue)
            {

                Venue venue = (Venue)_currentSelectedEntity;
                venue.InfectionRisk = float.Parse(InfectionRiskInputField.text); 
                if (_currentSelectedEntity is Workplace)
                {
                    Workplace workplace = (Workplace)_currentSelectedEntity;
                    
                    WorkplaceType workplaceType = (WorkplaceType) Enum.Parse(typeof(WorkplaceType), WorkplaceTypeDropdown.options[WorkplaceTypeDropdown.value].text);
                    int capacity = int.Parse(CapacityInputField.text);
                    workplace.WorkerCapacity = capacity;
                    workplace.Type = workplaceType;

                    if (_currentSelectedEntity is Hospital)
                    {
                        Hospital hospital = (Hospital)_currentSelectedEntity;
                        HospitalScale hospitalScale = (HospitalScale)Enum.Parse(typeof(HospitalScale), HospitalScaleDropdown.options[HospitalScaleDropdown.value].text);
                        WorkerAvailability workerAvailability = (WorkerAvailability)Enum.Parse(typeof(WorkerAvailability), WorkerAvailabilityDropdown.options[WorkerAvailabilityDropdown.value].text);

                        hospital.Scale = hospitalScale;
                        hospital.WorkerAvailability = workerAvailability;
                    }

                }
                else if (_currentSelectedEntity is Household)
                {
                    Household household = (Household)_currentSelectedEntity;
                    household.NumberOfPeople = byte.Parse(NumberOfPeopleInputField.text);
                    household.CarefulnessTendency = float.Parse(CarefulnessInputField.text);
                    household.PercentageOfWorkers = float.Parse(PercantageOfWorkersInputField.text);
                }
            }


        }
    }

    /// <summary>
    /// Method which deletes the current selected entities
    /// </summary>
    public void DeleteCurrentEntity() 
    {
        IEditorObject currentEditorObject = null;

        if (_currentSelectedEntity != null)
        {

            //Get current editor object
            //Get the edito object to save the UI name
            foreach (IEditorObject editorObject in editorObjects)
            {
                if (editorObject.EditorEntity == _currentSelectedEntity)
                {
                    currentEditorObject = editorObject;
                    break;
                }
            }

            if (currentEditorObject != null) 
            {

                //Destroy the gameObject in the scene
                GameObject gameObject = currentEditorObject.EditorGameObject;
                Destroy(gameObject);
                editorObjects.Remove(currentEditorObject);
                _currentSelectedEntity = null;
                //TODO disable save button and UI
                UIController.Instance.IsEntitySelectedUI(false);
            }
        }
 
    }

    // Start is called before the first frame update
    void Start()
    {
        SimulationGridManager.OnEditorObjectClicked += LoadEditorObjectUI;
    }
}
