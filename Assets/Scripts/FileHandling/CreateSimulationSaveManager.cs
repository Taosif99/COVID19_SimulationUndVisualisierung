using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FileHandling
{
    /// <summary>
    /// Class which implements the functionality of
    /// creating new simulation data.
    /// </summary>
    public class CreateSimulationSaveManager : MonoBehaviour
    {
        /// <summary>
        /// With this field the file name of the simulation will be acquired.
        /// </summary>
        [SerializeField] private TMP_InputField _nameInputField;

        /// <summary>
        /// TODO IMPLEMENT PROPER INPUT HANDLING
        /// </summary>
        private void CheckInputField()
        {


        }

        /// <summary>
        /// Methods which creates the simulation regarding the given input name.
        /// </summary>
        public void CreateSimulation()
        {
            CheckInputField();
            string fileName = _nameInputField.text;
            //////////////////////////////////////////////////////////////////////////////////////
            //Temporary Mocks for serialization, values must be get from global settings
            Simulation.Edit.Simulation simulation = FileHandler.GetSimulationMock();
            /////////////////////////////////////////////////////////////////////////////////
            FileHandler.SelectedFileName = fileName;
            FileHandler.SaveData(simulation);

        }


        public void CreateSimulationFromPrefab()
        {
            //TODO WHEN WE HAVE SOME PREFABS

        }
    }
}