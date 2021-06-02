using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorObjects;
using Grid;
using UnityEngine.UI;
/// <summary>
/// This class maintains the editor Objects during the Runtime
/// I think it would be better to implement a "gameMaster" class to remove Monobehaviour inheritance
/// </summary>
public class EditorObjectsManager : MonoBehaviour
{

    public GridManager SimulationGridManager;


    public List<IEditorObject> editorObjects = new List<IEditorObject>();
    public List<VenueEditorObject> venueEditorObjects = new List<VenueEditorObject>();
    public List<WorkplaceEditorObject> workplaceEditorObjects = new List<WorkplaceEditorObject>();
    public List<HospitalEditorObject> hospitalEditorObjects = new List<HospitalEditorObject>();
    public List<HouseholdEditorObject> householdEditorObjects = new List<HouseholdEditorObject>();


    //The settings objects can be outsourced in an UI class
    //public Text ObjectNameText;
    public InputField ObjectNameInputField;

    /// <summary>
    /// Method which adds an Editor object
    /// </summary>
    /// <param name="namedPrefab"></param>
    /// <param name="spawnPosition"></param>
    /// <param name="relativePositionInt"></param>
    /// <param name="planeWorldTransform"></param>
    /// <returns></returns>
    public GameObject AddEditorObject(GridManager.NamedPrefab namedPrefab, Vector3 spawnPosition, Vector3Int relativePositionInt,Transform planeWorldTransform)
    {

       PrefabName currentPrefabName = namedPrefab.prefabName;
       GameObject currentPrefabToSpawn = namedPrefab.prefab;
       IEditorObject editorObject = null;
       

        switch (currentPrefabName)
        {

            case PrefabName.Venue:
                editorObject = EditorObjectFactory.CreateVenueEditorObject(currentPrefabToSpawn, spawnPosition, relativePositionInt, planeWorldTransform);
                venueEditorObjects.Add((VenueEditorObject) editorObject);
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
                householdEditorObjects.Add((HouseholdEditorObject) editorObject);
                break;

            default:
                Debug.LogError("Unknown prefab name");
                break;
        }

        editorObjects.Add(editorObject);
        return editorObject.EditorGameObject;
    }


    //We may handle different venues according the properties what can be set
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
                return;
            }
        }
    
    }


    // Start is called before the first frame update
    void Start()
    {
        SimulationGridManager.OnEditorObjectClicked += LoadEditorObjectUI;



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
