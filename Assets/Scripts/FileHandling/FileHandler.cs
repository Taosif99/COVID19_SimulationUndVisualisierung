using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;
using Simulation.Edit;
using System.Collections.Generic;
using System;

namespace FileHandling
{

    /// <summary>
    /// Class which serializes a simulation with all entities.
    /// Simulation data will be loaded and saved in the Application.persistentDataPath + "\" + SaveStateFolderName Directory.
    /// The main advantage of thid directory is that it remains after updates and that
    /// it gives us platform independency. Screenshots will be saved in 
    /// Application.persistentDataPath + "\" + ScreenshotFolderName
    /// 
    /// In Windows: %userprofile%\AppData\LocalLow\<companyname>\<productname>
    /// <see cref="https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html"/> 
    /// </summary>
    public static class FileHandler
    {


        public static string SelectedFileName = null;
        private const string FileExtension = ".covidSim";
        private const string SaveStateFolderName = "SavedSimulations";
        private const string ScreenshotFolderName = "Screenshots";
        private const string DayLogFolderName = "SimulationLogs";
        
        //TODO CATCH  UnauthorizedAccessException

        #region serialization
        /// <summary>
        /// Method which saves simulation object as serialized object
        /// with the extension .covidSim. Using the same file name leads to
        /// overwritting. 
        /// </summary>
        /// <param name="simulation">The simulation object</param>
        public static void SaveData(Simulation.Edit.Simulation simulation)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = GetCurrentSaveStateFilePath();
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, simulation);
            stream.Close();
            Debug.Log("Data saved in: " + path);
        }

        /// <summary>
        /// Method to load Simulation Datat.
        /// </summary>
        /// <returns>The simulation object if operation was successful, else null</returns>
        public static Simulation.Edit.Simulation LoadData()
        {
            string path = GetCurrentSaveStateFilePath();
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                Simulation.Edit.Simulation data = formatter.Deserialize(stream) as Simulation.Edit.Simulation;
                stream.Close();
                return data;
            }
            else
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// Method which deletes the current selected simulation file.
        /// </summary>
        public static void DeleteData()
        {
            string path = GetCurrentSaveStateFilePath();
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

        /// <summary>
        /// Method which checks if a save file does already exist.
        /// (using the current selected file)
        /// </summary>
        /// <returns>true if it exists, else false.</returns>
        public static bool SaveFileExists()
        {
            string path = GetCurrentSaveStateFilePath();
            return File.Exists(path);
        }

        /// <summary>
        /// Method which checks if a save file does already exist.
        /// (using a filename as parameter) 
        /// </summary>
        /// <param name="filename">The filename to check without extension</param>
        /// <returns>true if this file exists, else false</returns>
        public static bool SaveFileExists(string filename)
        {
            //string path = Application.persistentDataPath + "/" + filename + FileExtension;
            string pathToFolder = GetSaveStatesFilePath();
            string path = pathToFolder + "/" + filename + FileExtension;
            return File.Exists(path);

        }


        /// <summary>
        /// Method to get all file paths ordered b the modfification date (descending).
        /// </summary>
        /// <returns>A string list with oredered file directories.</returns>
        public static List<string> GetFilePathsOrderByLastModifiedDate()
        {
            string path = GetSaveStatesFilePath();
            List<string> filePaths = new List<string>();

            List<FileInfo> sortedFiles = new DirectoryInfo(path).GetFiles("*.covidSim")
                                                  .OrderByDescending(file => file.LastWriteTime)
                                                  .ToList();
            // Adding to the return list
            sortedFiles.ForEach(fileInfo => filePaths.Add(fileInfo.FullName));
            return filePaths;
        }


        /// <summary>
        /// Method to get all file names ordered b the modfification date (descending).
        /// </summary>
        /// <returns>A string list with oredered file names.</returns>
        public static List<string> GetFileNamesOrderByLastModifiedDate()
        {
            List<string> filePaths = GetFilePathsOrderByLastModifiedDate();
            List<string> fileNames = new List<string>();
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                fileName = fileName.Remove(fileName.Length - FileExtension.Length);
                fileNames.Add(fileName);
            }


            return fileNames;
        }

        /// <summary>
        /// Method which returns the path where the screenshots are saved.
        /// Screenshots folder will be created if it does not exists.
        /// </summary>
        /// <returns>The path of the created/existing folder.</returns>
        public static string GetScreenshotFilePath()
        {
            string pathToFolder = Application.persistentDataPath + "/" + ScreenshotFolderName;
            Directory.CreateDirectory(pathToFolder);
            return pathToFolder;

        }

        /// <summary>
        /// Method which returns the path where the savestates are saved.
        /// Savestates folder will be created if it does not exists.
        /// </summary>
        /// <returns>The path of the created/existing folder.</returns>
        public static string GetSaveStatesFilePath()
        {
            string pathToFolder = Application.persistentDataPath + "/" + SaveStateFolderName;
            Directory.CreateDirectory(pathToFolder);
            return pathToFolder;

        }

        /// <summary>
        /// Method to get the path of the curren save state file.
        /// </summary>
        /// <returns>The corresponding full path with file extension.</returns>
        public static string GetCurrentSaveStateFilePath()
        {
            string pathToFolder = GetSaveStatesFilePath();
            return pathToFolder + "/" + SelectedFileName + FileExtension;
        }

        /// <summary>
        /// Method which write the gerated datasets to a csv file.
        /// </summary>
        /// <param name="dayInfo">the real world date the simulation started.</param>
        /// <param name="dataString">The whole dataset as line string representation csv with ; as seperator</param>
        public static void WriteToCsv(string dataString, DateTime playDate)
        {
            
            //ISO 8601 format 
            string pathToFolder = Application.persistentDataPath + "/" + DayLogFolderName;
            Directory.CreateDirectory(pathToFolder);
            string pathAndFileName = pathToFolder + "/"+"DayLog_"+ SelectedFileName+"_"+playDate.ToString("yyyyMMddTHHmmss") +".csv";
            File.WriteAllText(pathAndFileName, dataString);
        }




        #region Debug Mock
        /// <summary>
        /// Method to create a simulation Object for testing puropses. TODO REMOVE
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static Simulation.Edit.Simulation GetSimulationMock(Entity[] entities = null)
        {
            Policies policiesMock = new Policies(MaskType.None);
            Simulation.Edit.Event[] eventsMock = null;
            SimulationOptions simulationOptions = new SimulationOptions(policiesMock, eventsMock,null);
            Simulation.Edit.Simulation simulation = new Simulation.Edit.Simulation(simulationOptions, entities);
            return simulation;
        }
        #endregion


        //"Real" mocks
        public static Simulation.Edit.Simulation GetDefaultSimulationMock(Entity[] entities = null)
        {
            Policies policiesMock = new Policies(MaskType.None);
            Simulation.Edit.Event[] eventsMock = null;
            SimulationOptions simulationOptions = new SimulationOptions(policiesMock, eventsMock, new Simulation.Edit.AdjustableSimulationSettings());
            Simulation.Edit.Simulation simulation = new Simulation.Edit.Simulation(simulationOptions, entities);
            return simulation;
        }

        public static Simulation.Edit.Simulation GetDefaultSimulationMock(Entity[] entities, Simulation.Edit.AdjustableSimulationSettings adjustableSimulationSettings)
        {

            return null;
        
        }


    }
}