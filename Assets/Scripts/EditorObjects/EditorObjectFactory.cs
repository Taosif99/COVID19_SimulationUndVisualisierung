using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation.Edit;
using Simulation;

namespace EditorObjects
{

    /// <summary>
    /// This class implements a Factory for the editor objects
    /// which can be seen in the scene.
    /// </summary>
    public class EditorObjectFactory
    {
        
        public static IEditorObject Create(Entity entity,string UIName, GameObject prefabToSpawn) 
        {
            switch (entity)
            {

                case Hospital obj:
                   return CreateHospitalEditorObject(obj,UIName,prefabToSpawn);
                case  Household obj:
                    return CreateHouseholdEditorObject(obj,UIName,prefabToSpawn);
                case Workplace obj:
                    return CreateWorkplaceEditorObject(obj,UIName,prefabToSpawn);
              

            }

            throw new NotSupportedException($"Unsupported Editor Entity object type: " + entity.GetType());

        }



        public static WorkplaceEditorObject CreateWorkplaceEditorObject(Workplace workplace,string UIName, GameObject prefabToSpawn)
        {
            // Instantiate at finalPosition and zero rotation.
            GameObject gameObject = UnityEngine.Object.Instantiate(prefabToSpawn,Vector3.zero ,Quaternion.identity);
            gameObject.name = PrefabName.Workplace.ToString();
            WorkplaceEditorObject workplaceEditorObject = new WorkplaceEditorObject(gameObject, workplace, UIName);
            return workplaceEditorObject;

        }

        public static HouseholdEditorObject CreateHouseholdEditorObject(Household household, string UIName, GameObject prefabToSpawn)
        {
            // Instantiate at finalPosition and zero rotation.
            GameObject gameObject = UnityEngine.Object.Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
            gameObject.name = PrefabName.Household.ToString();
            HouseholdEditorObject householdEditorObject = new HouseholdEditorObject(gameObject, household, UIName);
            return householdEditorObject;
        }


        public static HospitalEditorObject CreateHospitalEditorObject(Hospital hospital, string UIName, GameObject prefabToSpawn)
        {
            // Instantiate at finalPosition and zero rotation.
            GameObject gameObject = UnityEngine.Object.Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
            gameObject.name = PrefabName.Hospital.ToString();
            HospitalEditorObject houpitalEditorObject = new HospitalEditorObject(gameObject, hospital, UIName);
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