using UnityEngine;
using EditorObjects;
using Simulation.Edit;

namespace Grid
{
    /// <summary>
    /// Class to provide the current selected model and
    /// to manage the possible models.
    /// </summary>
    public class ModelSelector : MonoBehaviour
    {
        public static ModelSelector Instance;

        //The used models
        public NamedPrefab[] Prefabs;

        //Will be set in the inspector
        public Transform ModelParentTransform;

        //Prefab of the counter of the Models
        public GameObject CounterPrefab;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        
        /// <summary>
        /// Retrieve the prefab for the given name/type.
        /// </summary>
        /// <param name="prefabName">Name/type of the prefab to retrieve</param>
        /// <returns>The prefab for the given name</returns>
        public GameObject GetPrefab(PrefabName prefabName)
        {
            foreach (NamedPrefab namedPrefab in Prefabs)
            {
                if (prefabName.Equals(namedPrefab.prefabName))
                {
                    return namedPrefab.prefab;
                }
            }

            return null;
        }
        
        /// <summary>
        /// Retrieve the prefab for the given editor entity.
        /// </summary>
        /// <param name="entity">Editor entity to retrieve the prefab for</param>
        /// <returns>The prefab for the given editor entity</returns>
        public GameObject GetPrefab(Entity entity)
        {
            switch (entity)
            {
                /*case Graph graph:
                    return GetPrefab(PrefabName.Graph);*/
                case Household household:
                    return GetPrefab(PrefabName.Household);
                case Hospital hospital:
                    return GetPrefab(PrefabName.Hospital);
                case Workplace workplace:
                    return GetPrefab(PrefabName.Workplace);
            }

            return null;
        }
    }
}