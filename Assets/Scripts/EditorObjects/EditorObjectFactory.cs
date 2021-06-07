using System;
using UnityEngine;
using Simulation.Edit;
using Grid;

namespace EditorObjects
{

    /// <summary>
    /// This class implements a Factory for the editor objects
    /// which can be seen in the scene.
    /// </summary>
    public static class EditorObjectFactory
    {

        public static IEditorObject Create(Entity entity, string UIName)
        {
            GameObject prefabToSpawn = ModelSelector.Instance.CurrentPrefabToSpawn.prefab;
            IEditorObject editorObject = null;

            switch (entity)
            {
                case Hospital obj:
                    editorObject = CreateHospitalEditorObject(obj, UIName, prefabToSpawn);
                    break;
                case Household obj:
                    editorObject = CreateHouseholdEditorObject(obj, UIName, prefabToSpawn);
                    break;
                case Workplace obj:
                    editorObject = CreateWorkplaceEditorObject(obj, UIName, prefabToSpawn);
                    break;
                default:
                    throw new NotSupportedException($"Unsupported Editor Entity object type: " + entity.GetType());

            }
            //Setting up the parent of the gameObject 
            editorObject.EditorGameObject.transform.parent = ModelSelector.Instance.ModelParentTransform;

            return editorObject;
        }



        public static WorkplaceEditorObject CreateWorkplaceEditorObject(Workplace workplace, string UIName, GameObject prefabToSpawn)
        {
            // Instantiate at zero point and zero rotation.
            GameObject gameObject = UnityEngine.Object.Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
            gameObject.name = PrefabName.Workplace.ToString();
            WorkplaceEditorObject workplaceEditorObject = new WorkplaceEditorObject(gameObject, workplace, UIName);
            return workplaceEditorObject;

        }

        public static HouseholdEditorObject CreateHouseholdEditorObject(Household household, string UIName, GameObject prefabToSpawn)
        {
            // Instantiate at zero point and zero rotation.
            GameObject gameObject = UnityEngine.Object.Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
            gameObject.name = PrefabName.Household.ToString();
            HouseholdEditorObject householdEditorObject = new HouseholdEditorObject(gameObject, household, UIName);
            return householdEditorObject;
        }


        public static HospitalEditorObject CreateHospitalEditorObject(Hospital hospital, string UIName, GameObject prefabToSpawn)
        {
            // Instantiate at zero point and zero rotation.
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