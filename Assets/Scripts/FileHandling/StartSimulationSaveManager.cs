using System.Collections.Generic;
using DialogBoxSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
        [FormerlySerializedAs("_buttonItem")]
        [SerializeField]
        private GameObject _simulationEntryPrefab;

        [FormerlySerializedAs("_panelGameObject")]
        [SerializeField]
        private GameObject _simulationPanel;

        [FormerlySerializedAs("SimSceneLoader")]
        [SerializeField]
        private SceneLoader _simSceneLoader;

        private void Start()
        {
            FileHandler.SelectedFileName = null;
            LoadSimulationEntries();
        }
        
        private void LoadSimulationEntries()
        {
            DestroyEntries();
            
            //Getting all simulation Names ordered by last modified date
            List<string> fileNames = FileHandler.GetFileNamesOrderByLastModifiedDate();
            
            foreach (string fileName in fileNames)
            {
                GameObject entry = Instantiate(_simulationEntryPrefab, _simulationPanel.transform);
                GameObject startButtonObj = entry.transform.GetChild(1).gameObject;
                
                //Change the text
                TMP_Text startButtonText = startButtonObj.transform.GetComponentInChildren<TMP_Text>();
                startButtonText.text = fileName;

                //Adding the listener <-> use uiController for uniformity
                Button startButton = startButtonObj.transform.GetComponent<Button>();
                AddStartButtonListener(startButton, fileName);
                AddDeleteButtonListener(entry, fileName);
            }
        }

        private void DestroyEntries()
        {
            for (int i = 0; i < _simulationPanel.transform.childCount; i++)
            {
                Destroy(_simulationPanel.transform.GetChild(i).gameObject);
            }
        }

        private void AddStartButtonListener(Button button, string fileName)
        {
            button.onClick.AddListener(() =>
            {
                FileHandler.SelectedFileName = fileName;
                _simSceneLoader.LoadSimulation();
            });
        }

        private void AddDeleteButtonListener(GameObject simulationButtonItem, string fileName)
        { 
            GameObject deleteGameObject = simulationButtonItem.transform.GetChild(0).gameObject;
            Button deleteButton = deleteGameObject.GetComponent<Button>();

            deleteButton.onClick.AddListener(() =>
            {
                FileHandler.SelectedFileName = fileName;
                
                string msg = "Are you sure you want to delete the file ? ";
                string name = "Delete file ?";

                DialogBox dialogBox = new DialogBox(name, msg);
                dialogBox.OnConfirmationPressed += () =>
                {
                    FileHandler.DeleteData();
                    LoadSimulationEntries();
                };

                DialogBoxManager.Instance.HandleDialogBox(dialogBox);
            });
        }
    }
}