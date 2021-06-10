using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Simulation.Edit;


namespace FileHandling
{

    /// <summary>
    /// Class which serializes a simulation with all entities.
    /// Data will be loaded and saved in the Application.persistentDataPath Directory.
    /// The main advantage of thid directory is that it remains after updates and that
    /// it gives us platform independency.
    /// 
    /// In Windows: %userprofile%\AppData\LocalLow\<companyname>\<productname>
    /// (See: https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html)
    /// </summary>
    public static class FileHandler 
    {

        public const string FileExtension = ".covidSim";
        public static string SelectedFileName = null;

        //TODO CATCH  UnauthorizedAccessException

        /// <summary>
        /// Method which saves simulation object as serialized object
        /// with the extension .covidSim. Using the same file name leads to
        /// overwritting. 
        /// </summary>
        /// <param name="simulation">The simulation object</param>
        public static bool  SaveData(Simulation.Edit.Simulation simulation)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + SelectedFileName + FileExtension;
            bool saveIsPermitted = true;
            
            if (File.Exists(path))
            {
                string overWriteMsg = "File with the same name already exists. Will you overwrite " + SelectedFileName + "?";
                //TODO DIALOG BOX

            }

            if (saveIsPermitted)
            {
                FileStream stream = new FileStream(path, FileMode.Create);
                formatter.Serialize(stream, simulation);
                stream.Close();
                //Debug.Log("Data saved in: " + path);
            }

            return saveIsPermitted;
        }

        /// <summary>
        /// Method to load Simulation Datat.
        /// </summary>
        /// <returns></returns>
        public static Simulation.Edit.Simulation LoadData()
        {
            string path = Application.persistentDataPath + "/" + SelectedFileName + FileExtension;
            string dialogMessage = "";
            
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                Simulation.Edit.Simulation data = formatter.Deserialize(stream) as Simulation.Edit.Simulation;
                stream.Close();
                dialogMessage = "Data loaded successfully!";
                //Debug.Log("Data loaded successfully!");
                return data;
            }
            else
            {

                dialogMessage = "Save file not found in " + path;
                Debug.LogError(dialogMessage);
                return null;
            }
        }

        /// <summary>
        /// Method which deletes the current selected simulation file.
        /// </summary>
        public static bool DeleteData()
        {
            string deleteMsg = "Are you sure you want to delete " + SelectedFileName + "?";

            //TODO DIALOG BOX
            bool deleteSimulation = true;

            if (deleteSimulation)
            {
                string path = Application.persistentDataPath + "/" + SelectedFileName + FileExtension;
                if (File.Exists(path))
                {
                    File.Delete(path);
                    Debug.Log("File deleted !");
                }
                else
                {
                    Debug.LogError("Save file to delete not found in " + path);
                }

            }

            return deleteSimulation;
        }


        #region Debug Mock
        /// <summary>
        /// Method to create a simulation Object for testing puropses.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static Simulation.Edit.Simulation GetSimulationMock(Entity[] entities = null)
        {
            Policies policiesMock = new Policies(MaskType.None);
            Simulation.Edit.Event[] eventsMock = null;
            SimulationOptions simulationOptions = new SimulationOptions(policiesMock, eventsMock);
            Simulation.Edit.Simulation simulation = new Simulation.Edit.Simulation(simulationOptions, entities);
            return simulation;
        }

        #endregion
    }
}