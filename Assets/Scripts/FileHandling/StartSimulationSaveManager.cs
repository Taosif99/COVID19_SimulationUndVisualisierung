using System.Collections.Generic;
using DialogBoxSystem;
using UnityEngine;
using UnityEngine.UI;


namespace FileHandling
{

    /// <summary>
    /// Class which handles the loading of simulations names
    /// in the StartSimulation and which sets up the name for the clicked simulation to load.
    /// </summary>
    public class StartSimulationSaveManager : MonoBehaviour
    {

        /// <summary>
        /// The prefab of a button must be a GameObject with:
        /// A delete button as first child.
        /// A load Simulation Button as second child.
        /// </summary>
        [SerializeField] private GameObject _buttonItem;
        
        //List which is used to delete/reload buttons
        private List<GameObject> _itemList = new List<GameObject>();

        //Variables for simulation Loading (StartSimulation Scene)
        
        
        //FIXME THIS PANEL SCROLLBAR SYSTEM IS KINDA BROKEN
        [SerializeField] private GameObject _panelGameObject;
        [SerializeField] private float _yPositionButtonChange = 60f;
        private float _yPositionPanelChange = 100f; // system must be improved dynamically



        public static StartSimulationSaveManager Instance;

        public SceneLoader SimSceneLoader;



        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            //SpawnButtonItemTest();
            FileHandler.SelectedFileName = null;
            LoadSimulationButtons();
        }


        public void LoadSimulationButtons()
        {
            ClearButtonItems();
            float yButtonPositionDifference = 0f;
            float yPanelDownGrowth = 0f;
            RectTransform panelRecTransform = _panelGameObject.GetComponent<RectTransform>();
            //Getting all simulation Names ordered by last modified date
            List<string> fileNames = FileHandler.GetFileNamesOrderByLastModifiedDate();
            foreach (string fileName in fileNames)
            {
                //Place buttons correctly
                GameObject simulationButtonItem = Instantiate(_buttonItem, _panelGameObject.transform);
                _itemList.Add(simulationButtonItem);
                simulationButtonItem.transform.position += new Vector3(0, yButtonPositionDifference, 0);
                yButtonPositionDifference -= _yPositionButtonChange;


                GameObject simulationButtonGameObject = simulationButtonItem.transform.GetChild(1).gameObject;
                //Change the text
                Text simulationButtonText = simulationButtonGameObject.transform.GetComponentInChildren<Text>();

                simulationButtonText.text = fileName;

                //Adding the listener <-> use uiController for uniformity
                Button button = simulationButtonGameObject.transform.GetComponent<Button>();
                AddButtonListenerStartSimulation(button, fileName);
                AddButtonListenerDeleteSimulation(simulationButtonItem,fileName);
                yPanelDownGrowth = yPanelDownGrowth - _yPositionPanelChange;
            }

            //Let the button item panel grow
            panelRecTransform.offsetMin = new Vector2(panelRecTransform.offsetMin.x, yPanelDownGrowth);
        }



        #region outsource to UI controller

        /// <summary>
        /// Method to destroy all previous loaded Bitton objects.
        /// </summary>
        private void ClearButtonItems()
        {
            foreach (GameObject buttonItemGameObject in _itemList)
            {
                Destroy(buttonItemGameObject);
            }
            _itemList.Clear();
        }


        /// <summary>
        /// Method which assigns the appropiate method to open a new scene 
        /// and setting up the filename.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="fileName"></param>
        private void AddButtonListenerStartSimulation(Button button, string fileName)
        {
            button.onClick.AddListener(() =>
            {
                FileHandler.SelectedFileName = fileName;
                SimSceneLoader.LoadSimulation();
            });
        }


        private void AddButtonListenerDeleteSimulation(GameObject simulationButtonItem, string fileName)
        { 
          GameObject deleteGameObject = simulationButtonItem.transform.GetChild(0).gameObject;
          Button deleteButton = deleteGameObject.GetComponent<Button>();


            deleteButton.onClick.AddListener(() =>
            {
                FileHandler.SelectedFileName = fileName;
                
                string msg = "Are you sure you want to delete the file ? ";
                string name = "Delete file ?";

                DialogBox dialogBox = new DialogBox(name, msg);
                dialogBox.OnConfirmationPressed += CreateSureToDeleteAction;

                DialogBoxManager.Instance.HandleDialogBox(dialogBox);

            });

        }

        /// <summary>
        /// Method which defines the deletion on confirmation and which reloads
        /// the buttons in the UI.
        /// </summary>
        private  void CreateSureToDeleteAction()
        {
            FileHandler.DeleteData();
            LoadSimulationButtons();
        }


        #endregion

        #region testing
        /// <summary>
        /// The scroll system must be improved.
        /// </summary>
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
        #endregion
    }
}