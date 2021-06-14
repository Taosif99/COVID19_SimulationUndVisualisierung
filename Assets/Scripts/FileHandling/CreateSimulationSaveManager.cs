using UnityEngine;
using DialogBoxSystem;
using TMPro;
using System.Linq;
using System;

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
        /// TODO IMPLEMENT PROPER INPUT HANDLING, just checking if string is empty and removing white spaces
        /// </summary>
        private bool CheckInputField()
        {
            string trimmedText =  string.Concat(_nameInputField.text.Where(c => !char.IsWhiteSpace(c)));
            return !trimmedText.Equals("");
        }

        /// <summary>
        /// Methods which creates the simulation regarding the given input name.
        /// </summary>
        public void CreateSimulation()
        {
            if (CheckInputField())
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
                _nameInputField.image.color = Color.red;

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