using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation.Edit;


namespace EditorObjects
{

    /// <summary>
    /// This class implements a Factory for the editor objects
    /// which can be seen in the scene.
    /// </summary>
    public static class EditorObjectFactory
    {


        //Some mock runtime Objects for testing, will be removed Later, todo uniform constructors
        private static Venue venueMock = new Venue(null,0.5f);
        private static Workplace workplaceMock = new Workplace(null,0.3f,WorkplaceType.Store, 20); //Check type definition
        private static Household householdMock = new Household(null, 0.4f,10,0.5f,0.7f,0.4f,10,6);
        private static Hospital hospitalMock = new Hospital(null,0.2f,WorkplaceType.Hospital, 3000,HospitalScale.Medium,WorkerAvailability.Low);


        public static VenueEditorObject CreateVenueEditorObject(GameObject prefabToSpawn, Vector3 spawnPosition, Vector3Int relativePosition, Transform parent = null)
        {
   
            Venue runtimeVenue = venueMock;
            //Must be get from the UI Controller / or use first default name
            string venueUIName = "venue mock name";
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

            Workplace runtimeWorkplace = workplaceMock;
            //Must be get from the UI Controller
            string workplaceUIName = "workplace mock name";
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

            Household runtimeHousehold = householdMock;
            //Must be get from the UI Controller
            string householdUIName = "household mock name";
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

            Hospital runtimeHospital = hospitalMock;
            //Must be get from the UI Controller
            string householdUIName = "hospital mock name";
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