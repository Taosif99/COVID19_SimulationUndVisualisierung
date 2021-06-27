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
                    dialogBox.OnConfirmationPressed += () =>
                    {
                        Simulation.Edit.Simulation simulation = FileHandler.GetDefaultSimulationMock();
                        FileHandler.SaveData(simulation);
                        SceneLoader.Instance.LoadSimulation();
                    };

                    DialogBoxManager.Instance.HandleDialogBox(dialogBox);
                    _nameInputField.image.color = Color.red;
                }
                else
                {   //save normally
                    Simulation.Edit.Simulation simulation = FileHandler.GetDefaultSimulationMock();
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

        public void CreateSimulationFromPrefab()
        {
            //TODO WHEN WE HAVE SOME PREFABS

        }
    }
}