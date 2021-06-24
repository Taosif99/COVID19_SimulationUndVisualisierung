using UnityEngine;
using EditorObjects;
using DialogBoxSystem;
using Simulation.Edit;
using System.Collections.Generic;
using Grid;
using TMPro;
using InputValidation;

namespace FileHandling
{

    /// <summary>
    /// Class which mainly contains methods concerning saving/loading from
    /// the savefile in the simulation scene.
    /// </summary>
    public class SimulationSaveManager : MonoBehaviour
    {

        [Header("References to managers")]
        [SerializeField] private EditorObjectsManager editorObjectsManager;
        [SerializeField] private GridManager _gridManager;


        private void Start()
        {
            string fileName = FileHandler.SelectedFileName;
            if (fileName != null)
            {
                Debug.Log("File to load: " + fileName);
                LoadFromFile();
            }
        }


        /// <summary>
        /// Method which initiates the saving process of the program configuration.
        /// </summary>

        public void SaveToFile()
        {
            List<IEditorObject> editorObjects = editorObjectsManager.GetAllEditorObjects();
            Entity[] entities = new Entity[editorObjects.Count];
            int index = 0;
            foreach (IEditorObject editorObject in editorObjects)
            {
                Entity entity = editorObject.EditorEntity;
                entities[index] = entity;
                index++;
            }
            //TODO REPLACE MOCK WITH REAL SIMULATION
            //Simulation.Edit.Simulation simulation = FileHandler.GetSimulationMock(entities);

            Simulation.Edit.Simulation simulation = SimulationMaster.Instance.CurrentSimulation ;
            simulation.Entities = entities;

            FileHandler.SaveData(simulation);
        }

        /// <summary>
        /// Method which will be called if user selects in the previous scene to load a file.
        /// </summary>
        public void LoadFromFile()
        {

            //Clear old scene and load data -> can be removed

            foreach (IEditorObject editorObject in editorObjectsManager.EditorObjectsDic.Values)
            {

                //TODO METHOD FOR THIS REPITITION
                GameObject gameObject = editorObject.EditorGameObject;
                Destroy(gameObject);
            }

            //reset naming counters 
            editorObjectsManager.WorkPlaceCounter = 1;
            editorObjectsManager.HospitalCounter = 1;
            editorObjectsManager.HouseholdCounter = 1;
            editorObjectsManager.EditorObjectsDic.Clear();
            editorObjectsManager.CurrentSelectedEntity = null;
            //Load Simulation
            Simulation.Edit.Simulation simulation = FileHandler.LoadData();
          
            //Not Found dialog box Todo
            if (simulation != null)
            {
                SimulationMaster.Instance.CurrentSimulation = simulation;
                Entity[] entities = simulation.Entities;
                if (entities != null)
                {
                    foreach (Entity entity in entities)
                    {
                        IEditorObject editorObject = EditorObjectFactory.Create(entity, "serialized mock text");
                        editorObjectsManager.AddEditorObjectToCollection(entity.Position, editorObject);
                        Vector2Int gridCellPosition = new Vector2Int(entity.Position.X, entity.Position.Y);
                        //Spawn in position-> spawn handler or manager
                        GameObject gameObject = editorObject.EditorGameObject;
                        _gridManager.PositionObjectInGrid(gameObject, gridCellPosition);
                    }
                    
                    UIController.Instance.IsEntitySelectedUI(false);
                }
                else Debug.Log("No entities !");
            }
            else
            {
                //This is not working...
                /*
                Debug.Log("File not found !");
                string msg = "Save file not found !";
                string name = "File not found";
                DialogBox dialogBox = new DialogBox(name, msg);
                dialogBox.HasCancelButon = false;
                //DialogBoxManager.Instance.HandleDialogBox(dialogBox);
                //Go Back to main menu
                //dialogBox.OnConfirmationPressed += SceneLoader.Instance.LoadMainMenu;
            */
            }


        }



        public void BackToMainMenuSave()
        {
            //Get the current file 
            if (FileHandler.SaveFileExists())
            {
                string msg = "Do you want to save your changes?";
                string name = "Save file ?";
                DialogBox dialogBox = new DialogBox(name, msg);
                dialogBox.OnConfirmationPressed += ReturnToMainMenuSaveAction;
                dialogBox.OnCancelPressed += SceneLoader.Instance.LoadMainMenu;
                DialogBoxManager.Instance.HandleDialogBox(dialogBox);
            } // else TODO show some error
        }



        public void SaveAs()
        {
            string msg = "Please enter a new simulation name";
            string name = "Save as a new file ";
            DialogBox dialogBox = new DialogBox(name, msg);
            dialogBox.HasTextField = true;
            TMP_InputField inputfield = DialogBoxManager.Instance.InputfieldGB.GetComponent<TMP_InputField>();
            inputfield.text = FileHandler.SelectedFileName;
            inputfield.onValueChanged.AddListener(delegate { SaveAsInputFieldOnChange(); });
            dialogBox.OnConfirmationPressed += SaveAsConfirmationAcion;
            DialogBoxManager.Instance.HandleDialogBox(dialogBox);
            

        }

        private void SaveAsInputFieldOnChange()
        {
            TMP_InputField inputfield = DialogBoxManager.Instance.InputfieldGB.GetComponent<TMP_InputField>();
            string newName = inputfield.text;
            GameObject OkButton = DialogBoxManager.Instance.OkButtonGB;
            //Overriding of other files than current file not allowed
            if (!InputValidator.BasicInputFieldValidation(inputfield) || (FileHandler.SaveFileExists(newName) && !(newName == FileHandler.SelectedFileName)))
            {
                OkButton.SetActive(false);
                inputfield.image.color = Color.red;
            }
            else
            {
                OkButton.SetActive(true);
                inputfield.image.color = Color.white;
            }
        }

        private void SaveAsConfirmationAcion()
        {
            FileHandler.SelectedFileName = DialogBoxManager.Instance.InputfieldGB.GetComponent<TMP_InputField>().text;
            SaveToFile();
        }


        private void ReturnToMainMenuSaveAction()
        {
            SaveToFile();
            SceneLoader.Instance.LoadMainMenu();
        }
    }
}