using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Simulation.Edit;



/// <summary>
/// Class which serializes a Simulation with all entities.
/// </summary>
public static class SerializationExecutor 
{

    //TODO CATCH  UnauthorizedAccessException
    public static void SaveData(Simulation.Edit.Simulation simulation)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/covidSim.simulation";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, simulation);
        stream.Close();
        Debug.Log("Data saved in: " + path);
    }


    public static Simulation.Edit.Simulation LoadData()
    {
        string path = Application.persistentDataPath + "/covidSim.simulation";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Simulation.Edit.Simulation data = formatter.Deserialize(stream) as Simulation.Edit.Simulation;
            stream.Close();
            Debug.Log("Data loaded successfully!");
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }



}
