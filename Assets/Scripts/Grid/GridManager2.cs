using System.Collections.Generic;
using UnityEngine;
using EditorObjects;
using System;
using UnityEngine.EventSystems;

namespace Grid
{
    /// <summary>
    ///  Class which places objects to the grid.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    public class GridManager2 : MonoBehaviour
    {
        [SerializeField] private int _cellExtent = 10;
        private Mesh _plane;

        public Grid Grid { get; private set; }
        public int CellExtent => _cellExtent;


        //Manager which is responsible for maintainaing the editor objects
        public EditorObjectsManager EditObjectsManager;

        //The GameObject prefab to spawn with its name
        public NamedPrefab CurrentPrefabToSpawn { get; set; }
        public PrefabName CurrentPrefabName = PrefabName.None;

        //The used models
        public NamedPrefab[] Prefabs;

        //We use this hashset to make sure that a a object is placed only once
        private HashSet<Vector2> _placedPositions = new HashSet<Vector2>();

        //To Check if we clicked the correct layer
        [SerializeField] private LayerMask _groundMask;
        //To scale the used prefabs, temporarily
        [SerializeField] private float _scaleDiv = 2f;

        //Here methods can listen which need to know
        //if we clicked on an already existing object
        public Action<Vector3> OnEditorObjectClicked;


        private void Awake()
        {
            _plane = GetComponent<MeshFilter>().mesh;
            Grid = new Grid
            {
                CellSize = _plane.bounds.extents.x * transform.localScale.x / CellExtent
            };

            Debug.Log("Using cell size " + Grid.CellSize);
        }

        private void Update()
        {
            //Check if left mouse button clicked and UI not clicked
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                //Raycast into the scene
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Check if we hit something
                if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _groundMask))
                {
                    //Vector3 point = hitInfo.point;
                    //Debug.Log($"Relative Position On Grid: {new Vector2(point.x, point.z)} -> {Grid.GetGridCell(new Vector2(point.x, point.z))}");
                    PlacePrefab(hitInfo.point);
                }
            }
        }


        private void PlacePrefab(Vector3 clickPoint)
        {

            Vector2Int gridCellPosition = Grid.GetGridCell(new Vector2(clickPoint.x,clickPoint.z));
            Vector2 spawnPosition =  Grid.GetRelativeWorldPosition(gridCellPosition);

            if (!_placedPositions.Contains(gridCellPosition)) 
            {

                //Create a venue and add to editor objects

                //GameObject gameObject = EditObjectsManager.AddEditorObject(CurrentPrefabToSpawn, new Vector3(spawnPosition.x,0,spawnPosition.y), gridCellPosition,this.transform) ;


                GameObject gameObject = EditObjectsManager.AddEditorObject2(CurrentPrefabToSpawn, gridCellPosition, this.transform);
                gameObject.transform.position = new Vector3(spawnPosition.x, 0, spawnPosition.y);
                
                //TODO: Quick fix we need appropiate models or implement a system
                if (gameObject != null)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    gameObject.transform.localScale /= _scaleDiv;
                }
                _placedPositions.Add(gridCellPosition);

            }
            else
            {
                Debug.Log("Position already used !");
                OnEditorObjectClicked?.Invoke(new Vector3(spawnPosition.x, 0, spawnPosition.y));

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
                    Debug.Log("Set Prefab Name:" + namedPrefab.prefabName);
                    CurrentPrefabName = namedPrefab.prefabName;
                    return;
                }
            }
        }


        


    }
}
