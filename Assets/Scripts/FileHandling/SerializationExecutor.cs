using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

using Simulation.Edit;

namespace FileHandling
{

    /// <summary>
    /// Class which serializes a Simulation with all entities.
    /// Data will be load and saved in the Application.persistentDataPath Directory.
    /// 
    /// 
    /// In Windows: %userprofile%\AppData\LocalLow\<companyname>\<productname>
    /// (See: https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html)
    /// </summary>
    public static class SerializationExecutor  // Rename to file handling
    {

        public const string FileExtension = ".covidSim";
        public static string SelectedFileName = null;

        //TODO CATCH  UnauthorizedAccessException

        /// <summary>
        /// Method which saves a Simulation Object to a serialized object
        /// with the extension .covidSim. Using the same file name leads to
        /// overwritting
        /// </summary>
        /// <param name="simulation">The simulation object</param>
        public static void SaveData(Simulation.Edit.Simulation simulation)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + SelectedFileName + FileExtension;
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, simulation);
            stream.Close();
            //Debug.Log("Data saved in: " + path);
        }

        /// <summary>
        /// Method to load Simulation Datat.
        /// </summary>
        /// <returns></returns>
        public static Simulation.Edit.Simulation LoadData()
        {
            string path = Application.persistentDataPath + "/" + SelectedFileName + FileExtension;
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                Simulation.Edit.Simulation data = formatter.Deserialize(stream) as Simulation.Edit.Simulation;
                stream.Close();
                //Debug.Log("Data loaded successfully!");
                return data;
            }
            else
            {
                Debug.LogError("Save file not found in " + path);
                return null;
            }
        }


        public static void DeleteData()
        {

            //TODO

        }


        #region Debug
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