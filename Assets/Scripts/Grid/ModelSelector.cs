using UnityEngine;
using EditorObjects;

namespace Grid
{
    /// <summary>
    /// Class to provide the current selected model and
    /// to manage the possible models.
    /// </summary>
    public class ModelSelector : MonoBehaviour
    {

        public static ModelSelector Instance;

        //The GameObject prefab to spawn with its name
        public NamedPrefab CurrentPrefabToSpawn { get; set; }
        public PrefabName CurrentPrefabName = PrefabName.None;
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
        /// Method to set the current prefab outside this class.
        /// </summary>
        /// <param name="prefabName">The name of the prefab. </param>
        public void SetCurrentPrefab(PrefabName prefabName)
        {
            foreach (NamedPrefab namedPrefab in Prefabs)
            {
                if (prefabName.Equals(namedPrefab.prefabName))
                {
                    CurrentPrefabToSpawn = namedPrefab;
                    //Debug.Log("Set Prefab Name:" + namedPrefab.prefabName);
                    CurrentPrefabName = namedPrefab.prefabName;
                    return;
                }
            }
        }
    }
}