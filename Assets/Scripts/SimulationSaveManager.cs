using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// Class which handles loading and saving of simulation prefabs/configurations.
/// </summary>
public class SimulationSaveManager : MonoBehaviour
{



    //Variables for simulation Loading (StartSimulation Scene)

    //[SerializeField] private GameObject _VerticalButtonGroupParent;
    //[SerializeField] private GameObject _simulationButtonsPrefab;
    [SerializeField] private GameObject _panelGameObject;
    [SerializeField] private GameObject _buttonItem;
    [SerializeField] private float _yPositionButtonChange = 60f;
     private float _yPositionPanelChange = 100f; // Until 50 Buttons okay, system must be improved dynamically



    //The simulation filename the user has choosen to load
    private string _selectedFileName;
    public string SelectedFileName { get => _selectedFileName; set => _selectedFileName = value; }

    private void Awake()
    {
        //Making sure that game object is not destroyed if scene is changed
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        //SpawnButtonItemTest();
        LoadSimulationButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void SpawnButtonItemTest()
    {
        string mockName = "simulation";
        int amountButtons = 10;
       //yPositionPanelChange = amountButtons * 1.5f; //Quick fix to make sure that it works always
        float yButtonPositionDifference = 0f;
        float yPanelDownGrowth = 0f;
        RectTransform panelRecTransform = _panelGameObject.GetComponent<RectTransform>();

        for (int i = 0; i < amountButtons; i++)
        {
            //Place buttons correctly
            GameObject simulationButtonItem = Instantiate(_buttonItem,_panelGameObject.transform);
            simulationButtonItem.transform.position += new Vector3(0, yButtonPositionDifference, 0);
            yButtonPositionDifference -= _yPositionButtonChange;


            GameObject simulationButton = simulationButtonItem.transform.GetChild(1).gameObject;
            //Change the text
            Text simulationButtonText = simulationButton.transform.GetComponentInChildren<Text>();
            simulationButtonText.text = mockName;

            yPanelDownGrowth = yPanelDownGrowth -_yPositionPanelChange;
        }


        //Let the button item panel grow
        panelRecTransform.offsetMin = new Vector2(panelRecTransform.offsetMin.x, yPanelDownGrowth);
    }




    private void LoadSimulationButtons()
    {

        float yButtonPositionDifference = 0f;
        float yPanelDownGrowth = 0f;
        RectTransform panelRecTransform = _panelGameObject.GetComponent<RectTransform>();


        string path = Application.persistentDataPath;

        //Getting all simulation Names
        string[] filePaths = System.IO.Directory.GetFiles(path, "*.covidSim");

        foreach (string filePath in filePaths)
        {
            Debug.Log("Found file: " + filePath);

            //Place buttons correctly
            GameObject simulationButtonItem = Instantiate(_buttonItem, _panelGameObject.transform);
            simulationButtonItem.transform.position += new Vector3(0, yButtonPositionDifference, 0);
            yButtonPositionDifference -= _yPositionButtonChange;


            GameObject simulationButton = simulationButtonItem.transform.GetChild(1).gameObject;
            //Change the text
            Text simulationButtonText = simulationButton.transform.GetComponentInChildren<Text>();
             
            //Remove the file extension
            string simulationName = Path.GetFileName(filePath);
            simulationName = simulationName.Remove(simulationName.Length - SerializationExecutor.FileExtension.Length);

            simulationButtonText.text = simulationName;
            yPanelDownGrowth = yPanelDownGrowth - _yPositionPanelChange;
        }



    }



}