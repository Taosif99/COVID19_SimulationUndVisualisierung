using UnityEngine;
using DialogBoxSystem;
using TMPro;
using InputValidation;

namespace FileHandling
{
    /// <summary>
    /// Class which implements the functionality of
    /// creating new simulation data.
    /// 
    /// TODO: REPLACE SIMULATION MOCK OBJECTS
    /// </summary>
    public class CreateSimulationSaveManager : MonoBehaviour
    {
        /// <summary>
        /// With this field the file name of the simulation will be acquired.
        /// </summary>
        [SerializeField] private TMP_InputField _nameInputField;


        /// <summary>
        /// Methods which creates the simulation regarding the given input name.
        /// </summary>
        public void CreateSimulation()
        {
            if (InputValidator.BasicInputFieldValidation(_nameInputField))
            {
                _nameInputField.image.color = Color.white;
                string fileName = _nameInputField.text;
                FileHandler.SelectedFileName = fileName;
                if (FileHandler.SaveFileExists())
                {
                    string msg = "File with the same name already exists. Do you want to overwrite this file?";
                    string name = "File already exists";
                    DialogBox dialogBox = new DialogBox(name, msg);
                    dialogBox.OnConfirmationPressed += FileSaveExistsActionConfimation;
                    DialogBoxManager.Instance.HandleDialogBox(dialogBox);

                    _nameInputField.image.color = Color.red;
                }
                else
                {   //save normally
                    Simulation.Edit.Simulation simulation = FileHandler.GetSimulationMock();
                    FileHandler.SaveData(simulation);
                    SceneLoader.Instance.LoadSimulation();
                }
            }

            else
            {
                string msg = "Please enter a name!";
                string name = "No name entered";
                DialogBox dialogBox = new DialogBox(name, msg);
                dialogBox.HasCancelButon = false;
                DialogBoxManager.Instance.HandleDialogBox(dialogBox);
            }
        }
        /// <summary>
        /// Method which will be passed to the DialogBox confirmation action handler.
        /// The simulation will be overweritten if user confirms it.
        /// </summary>
        private void FileSaveExistsActionConfimation()
        {
            Simulation.Edit.Simulation simulation = FileHandler.GetSimulationMock();
            FileHandler.SaveData(simulation);
            SceneLoader.Instance.LoadSimulation();

        }


        public void CreateSimulationFromPrefab()
        {
            //TODO WHEN WE HAVE SOME PREFABS

        }
    }
}