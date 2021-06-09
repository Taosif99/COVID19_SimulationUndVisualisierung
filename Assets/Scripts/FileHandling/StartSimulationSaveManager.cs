using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace FileHandling
{

    /// <summary>
    /// Class which handles the loading of simulations names
    /// in the StartSimulation and which sets up the name for the clicked simulation to load.
    /// </summary>
    public class StartSimulationSaveManager : MonoBehaviour
    {



        //Variables for simulation Loading (StartSimulation Scene)
        [SerializeField] private GameObject _panelGameObject;
        [SerializeField] private GameObject _buttonItem;
        [SerializeField] private float _yPositionButtonChange = 60f;
        private float _yPositionPanelChange = 100f; // Until 50 Buttons okay, system must be improved dynamically


        public SceneLoader SimSceneLoader;


        // Start is called before the first frame update
        void Start()
        {
            //SpawnButtonItemTest();
            SerializationExecutor.SelectedFileName = null;
            LoadSimulationButtons();
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


                GameObject simulationButtonGameObject = simulationButtonItem.transform.GetChild(1).gameObject;
                //Change the text
                Text simulationButtonText = simulationButtonGameObject.transform.GetComponentInChildren<Text>();

                //Remove the file extension
                string simulationName = Path.GetFileName(filePath);
                simulationName = simulationName.Remove(simulationName.Length - SerializationExecutor.FileExtension.Length);

                simulationButtonText.text = simulationName;
                yPanelDownGrowth = yPanelDownGrowth - _yPositionPanelChange;

                //Adding the listener <-> use uiController for uniformity
                Button button = simulationButtonGameObject.transform.GetComponent<Button>();
                AddButtonListener(button, simulationName);
            }

            //Let the button item panel grow
            panelRecTransform.offsetMin = new Vector2(panelRecTransform.offsetMin.x, yPanelDownGrowth);
        }

        /// <summary>
        /// Method which assigns the appropiate method to open a new scene 
        /// and setting up the filename.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="simulationName"></param>
        private void AddButtonListener(Button button, string simulationName)
        {
            button.onClick.AddListener(() =>
            {
                SerializationExecutor.SelectedFileName = simulationName;
                SimSceneLoader.LoadSimulation();
            });
        }




        private void SpawnButtonItemTest()
        {
            string mockName = "simulation";
            int amountButtons = 5;
            //yPositionPanelChange = amountButtons * 1.5f; //Quick fix to make sure that it works always
            float yButtonPositionDifference = 0f;
            float yPanelDownGrowth = 0f;
            RectTransform panelRecTransform = _panelGameObject.GetComponent<RectTransform>();

            for (int i = 0; i < amountButtons; i++)
            {
                //Place buttons correctly
                GameObject simulationButtonItem = Instantiate(_buttonItem, _panelGameObject.transform);
                simulationButtonItem.transform.position += new Vector3(0, yButtonPositionDifference, 0);
                yButtonPositionDifference -= _yPositionButtonChange;
                GameObject simulationButton = simulationButtonItem.transform.GetChild(1).gameObject;
                //Change the text
                Text simulationButtonText = simulationButton.transform.GetComponentInChildren<Text>();
                simulationButtonText.text = mockName;
                yPanelDownGrowth = yPanelDownGrowth - _yPositionPanelChange;
            }
            //Let the button item panel grow
            panelRecTransform.offsetMin = new Vector2(panelRecTransform.offsetMin.x, yPanelDownGrowth);
        }
    }
}