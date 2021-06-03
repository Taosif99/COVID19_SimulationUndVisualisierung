using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorObjects;
using Grid;
using UnityEngine.UI;
using Simulation.Runtime;
using System.Linq;
/// <summary>
/// This class maintains the editor Objects during the Runtime
/// I think it would be better to implement a "gameMaster" class to remove Monobehaviour inheritance
/// </summary>
public class EditorObjectsManager : MonoBehaviour
{

    public GridManager SimulationGridManager;


    public List<IEditorObject> editorObjects = new List<IEditorObject>();

    //This lists are currently NOT used
    public List<VenueEditorObject> venueEditorObjects = new List<VenueEditorObject>();
    public List<WorkplaceEditorObject> workplaceEditorObjects = new List<WorkplaceEditorObject>();
    public List<HospitalEditorObject> hospitalEditorObjects = new List<HospitalEditorObject>();
    public List<HouseholdEditorObject> householdEditorObjects = new List<HouseholdEditorObject>();


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

            case PrefabName.Venue:
                editorObject = EditorObjectFactory.CreateVenueEditorObject(currentPrefabToSpawn, spawnPosition, relativePositionInt, planeWorldTransform);
                venueEditorObjects.Add((VenueEditorObject)editorObject);
                break;

            case PrefabName.Workplace:
                editorObject = EditorObjectFactory.CreateWorkplaceEditorObject(currentPrefabToSpawn, spawnPosition, relativePositionInt, planeWorldTransform);
                workplaceEditorObjects.Add((WorkplaceEditorObject)editorObject);
                break;

            case PrefabName.Hospital:
                editorObject = EditorObjectFactory.CreateHospitalEditorObject(currentPrefabToSpawn, spawnPosition, relativePositionInt, planeWorldTransform);
                hospitalEditorObjects.Add((HospitalEditorObject)editorObject);
                break;

            case PrefabName.Household:
                editorObject = EditorObjectFactory.CreateHouseholdEditorObject(currentPrefabToSpawn, spawnPosition, relativePositionInt, planeWorldTransform);
                householdEditorObjects.Add((HouseholdEditorObject)editorObject);
                break;

            default:
                Debug.LogError("Unknown prefab name");
                break;
        }

        editorObjects.Add(editorObject);
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
                ObjectNameInputField.text = editorObject.UIName;
                Entity entity = editorObject.RuntimeEntity;
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
                            WorkplaceTypeDropdown.value = availableWorkplaceOptions.IndexOf(workplace.WorkType.ToString());
                            CapacityInputField.text = workplace.WorkerCapacity.ToString();

                            if (entity is Hospital)
                            {
                                UIController.Instance.LoadHospitalUI();
                                //FIXME THAT DOES NOT WORK
                                Hospital hospital = (Hospital)entity;
                                List<string> availableHospitalScaleOptions = HospitalScaleDropdown.options.Select(option => option.text).ToList();
                                List<string> availableWorkerAvailabilityOptions = WorkerAvailabilityDropdown.options.Select(option => option.text).ToList();
                                HospitalScaleDropdown.value = availableHospitalScaleOptions.IndexOf(hospital.HospitalScale.ToString());
                                WorkerAvailabilityDropdown.value = availableWorkerAvailabilityOptions.IndexOf(hospital.HospitalWorkerAvailability.ToString());
                            }

                        }
                        else if (entity is Household)
                        {
                            UIController.Instance.LoadHouseholdUI();
                            Household household = (Household)entity;
                            //I need the saved Objects for that...
                            //NumberOfPeopleInputField
                            //CarefulnessInputField
                        }
                    }

                    UIController.Instance.UnselectSelectedButton();
                }
                return;
            }
        }

    }

    //Is added to the save button
    
    /// <summary>
    /// TODO Load save values to runtime entity
    /// </summary>
    public void SaveToEntity()
    {
        if (_currentSelectedEntity != null)
        {

            if (_currentSelectedEntity is Venue)
            {

                if (_currentSelectedEntity is Workplace)
                {
                    

                    if (_currentSelectedEntity is Hospital)
                    {
                        
                    }

                }
                else if (_currentSelectedEntity is Household)
                {
                    
                }
            }


        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SimulationGridManager.OnEditorObjectClicked += LoadEditorObjectUI;
    }
}
