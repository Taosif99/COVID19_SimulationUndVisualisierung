using System.Collections.Generic;
using UnityEngine;
using EditorObjects;
using System;

namespace Grid
{
    /// <summary>
    ///  Class which places objects to the grid.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    public class GridManager : MonoBehaviour
    {

        [SerializeField] private int _cellExtent = 10;
        private Mesh _plane;

        public Grid Grid { get; private set; }
        public int CellExtent => _cellExtent;



        //For access of loading object
        public HashSet<Vector2Int> PlacedPositions { get => _placedPositions; set => _placedPositions = value; }


        //Manager which is responsible for maintainaing the editor objects
        public EditorObjectsManager EditObjectsManager;

        //We use this hashset to make sure that a a object is placed only once
        private HashSet<Vector2Int> _placedPositions = new HashSet<Vector2Int>();

        //To Check if we clicked the correct layer
        [SerializeField] private LayerMask _groundMask;
        //To scale the used prefabs, temporarily
        [SerializeField] private float _scaleDiv = 2f;

        //Here methods can listen which need to know
        //if we clicked on an already existing object
        public Action<Vector2Int> OnEditorObjectClicked;


        public CameraMovement CameraController;

        private void Awake()
        {
            _plane = GetComponent<MeshFilter>().mesh;
            Grid = new Grid
            {
                CellSize = _plane.bounds.extents.x * transform.localScale.x / CellExtent
            };
            //Debug.Log("Plane bounds: " + _plane.bounds.extents.x);
            //Debug.Log("Using cell size " + Grid.CellSize);
        }

        // TODO: Move to EditorObjectsManager
        private void Update()
        {
            //Check if left mouse button clicked and UI not clicked, rethink prefab.None for UI logic
            if (Input.GetMouseButtonDown(0) && !CameraController.IsMouseOverUi)
            {
                //Raycast into the scene
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                //Check if we hit something
                if (!Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _groundMask))
                {
                    return;
                }
                
                var worldPoint = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
                Vector3 localPoint = worldPoint - transform.position;
                Vector2Int gridCellPosition = Grid.GetGridCell(new Vector2(localPoint.x, localPoint.z));
                    
                if (PlacedPositions.Contains(gridCellPosition))
                {
                    OnEditorObjectClicked?.Invoke(gridCellPosition);
                }
            }
        }

        //Implement Placement Handler eventually later

        /// <summary>
        /// Method to place an concrete GameObject to a grid cell position in the world map.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="gridCellPosition"></param>
        public void PositionObjectInGrid(GameObject gameObject, Vector2Int gridCellPosition)
        {
            Vector3 spawnPosition = Grid.GetRelativeWorldPosition(gridCellPosition);
            //TODO: Quick fix we need appropiate models or implement a system
            if (gameObject != null)
            {
                gameObject.transform.position = new Vector3(spawnPosition.x, 0, spawnPosition.y);
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                gameObject.transform.localScale /= _scaleDiv;
                //update counter position
                /*StateCounter counter = gameObject.GetComponent<StateCounter>();
                counter.InstantiateCounter(spawnPosition);*/
                PlacedPositions.Add(gridCellPosition);

            } //Todo else exception

        }
    }
}
