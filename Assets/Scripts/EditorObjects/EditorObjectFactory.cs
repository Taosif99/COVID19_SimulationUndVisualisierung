using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation.Runtime;


namespace EditorObjects
{

    /// <summary>
    /// This class implements a Factory for the editor objects
    /// which can be seen in the scene.
    /// </summary>
    public static class EditorObjectFactory
    {



        public static VenueEditorObject CreateVenueEditorObject(GameObject prefabToSpawn, Vector3 spawnPosition, Vector3Int relativePosition, Transform parent = null)
        {
            //must be get by the RuntimeObject Factory or some manager/controller
            Venue runtimeVenue = null;
            //Must be get from the UI Controller
            string venueUIName = "venue placeholder name";
            // Instantiate at finalPosition and zero rotation.
            GameObject gameObject = UnityEngine.Object.Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            gameObject.name = PrefabName.Venue.ToString();
            if (parent != null) gameObject.transform.parent = parent;

            //We may add here the required monobehaviours
            //...gameObject.AddComponent
            VenueEditorObject venueEditorObject = new VenueEditorObject(gameObject, runtimeVenue, relativePosition, venueUIName);

            return venueEditorObject;
        }


        public static WorkplaceEditorObject CreateWorkplaceEditorObject(GameObject prefabToSpawn, Vector3 spawnPosition, Vector3Int relativePosition, Transform parent = null)
        {
            //must be get by the RuntimeObject Factory
            Workplace runtimeWorkplace = null;
            //Must be get from the UI Controller
            string workplaceUIName = "workplace placeholder name";
            // Instantiate at finalPosition and zero rotation.
            GameObject gameObject = UnityEngine.Object.Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            gameObject.name = PrefabName.Workplace.ToString();
            if (parent != null) gameObject.transform.parent = parent;
            //We may add here the required monobehaviours
            //...gameObject.AddComponent
            WorkplaceEditorObject workplaceEditorObject = new WorkplaceEditorObject(gameObject, runtimeWorkplace, relativePosition, workplaceUIName);
            return workplaceEditorObject;
        }

        public static HouseholdEditorObject CreateHouseholdEditorObject(GameObject prefabToSpawn, Vector3 spawnPosition, Vector3Int relativePosition, Transform parent = null)
        {
            //must be get by the RuntimeObject Factory
            Household runtimeHousehold = null;
            //Must be get from the UI Controller
            string householdUIName = "household placeholder name";
            // Instantiate at finalPosition and zero rotation.
            GameObject gameObject = UnityEngine.Object.Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            gameObject.name = PrefabName.Household.ToString();
            if (parent != null) gameObject.transform.parent = parent;
            //We may add here the required monobehaviours
            //...gameObject.AddComponent
            HouseholdEditorObject householdEditorObject = new HouseholdEditorObject(gameObject, runtimeHousehold, relativePosition, householdUIName);
            return householdEditorObject;
        }


        public static HospitalEditorObject CreateHospitalEditorObject(GameObject prefabToSpawn, Vector3 spawnPosition, Vector3Int relativePosition, Transform parent = null)
        {
            //must be get by the RuntimeObject Factory
            Hospital runtimeHospital = null;
            //Must be get from the UI Controller
            string householdUIName = "household placeholder name";
            // Instantiate at finalPosition and zero rotation.
            GameObject gameObject = UnityEngine.Object.Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            gameObject.name = PrefabName.Hospital.ToString();
            if (parent != null) gameObject.transform.parent = parent;
            //We may add here the required monobehaviours
            //...gameObject.AddComponent
            HospitalEditorObject houpitalEditorObject = new HospitalEditorObject(gameObject, runtimeHospital, relativePosition, householdUIName);
            return houpitalEditorObject;
        }


        /// <summary>
        /// TODO WHEN GRAPH IS A OBJECT IN THE WORLD 
        /// </summary>
        public static void CreateGraphEditorObject()
        {
            throw new NotImplementedException();
        }
    }
}