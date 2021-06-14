using UnityEngine;
using EditorObjects;
using DialogBoxSystem;
using Simulation.Edit;
using System.Collections.Generic;
using Grid;


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
            Simulation.Edit.Simulation simulation = FileHandler.GetSimulationMock(entities);
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
                Entity[] entities = simulation.Entities;
                if (entities != null)
                {
                    foreach (Entity entity in entities)
                    {
                        if (entity is Workplace)
                        {
                            ModelSelector.Instance.SetCurrentPrefab(PrefabName.Workplace);

                            if (entity is Hospital)
                            {
                                ModelSelector.Instance.SetCurrentPrefab(PrefabName.Hospital);
                            }
                        }
                        else if (entity is Household)
                        {
                            ModelSelector.Instance.SetCurrentPrefab(PrefabName.Household);
                        }
                        IEditorObject editorObject = EditorObjectFactory.Create(entity, "serialized mock text");
                        editorObjectsManager.EditorObjectsDic.Add(entity.Position, editorObject);
                        Vector2Int gridCellPosition = new Vector2Int(entity.Position.X, entity.Position.Y);
                        //Spawn in position-> spawn handler or manager
                        GameObject gameObject = editorObject.EditorGameObject;
                        _gridManager.PositionObjectInGrid(gameObject, gridCellPosition);
                    }
                    ModelSelector.Instance.SetCurrentPrefab(PrefabName.None);
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

        private void ReturnToMainMenuSaveAction()
        {
            SaveToFile();
            SceneLoader.Instance.LoadMainMenu();
        }


    }
}