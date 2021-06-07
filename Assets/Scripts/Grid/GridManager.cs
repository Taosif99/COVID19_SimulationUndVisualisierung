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
    public class GridManager : MonoBehaviour
    {

        [SerializeField] private int _cellExtent = 10;
        private Mesh _plane;

        public Grid Grid { get; private set; }
        public int CellExtent => _cellExtent;


        //Manager which is responsible for maintainaing the editor objects
        public EditorObjectsManager EditObjectsManager;

        //We use this hashset to make sure that a a object is placed only once
        private HashSet<Vector2> _placedPositions = new HashSet<Vector2>();

        //To Check if we clicked the correct layer
        [SerializeField] private LayerMask _groundMask;
        //To scale the used prefabs, temporarily
        [SerializeField] private float _scaleDiv = 2f;

        //Here methods can listen which need to know
        //if we clicked on an already existing object
        public Action<Vector2Int> OnEditorObjectClicked;

        private void Awake()
        {
            _plane = GetComponent<MeshFilter>().mesh;
            Grid = new Grid
            {
                CellSize = _plane.bounds.extents.x * transform.localScale.x / CellExtent
            };
            Debug.Log("Plane bounds: " + _plane.bounds.extents.x);
            Debug.Log("Using cell size " + Grid.CellSize);
        }

        private void Update()
        {
            //Check if left mouse button clicked and UI not clicked
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && ModelSelector.Instance.CurrentPrefabName != PrefabName.None)
            {
                //Raycast into the scene
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Check if we hit something
                if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _groundMask))
                {
                    PlacePrefab(hitInfo.point);
                }
            }
        }


        private void PlacePrefab(Vector3 clickPoint)
        {

            Vector2Int gridCellPosition = Grid.GetGridCell(new Vector2(clickPoint.x, clickPoint.z));
            Vector2 spawnPosition = Grid.GetRelativeWorldPosition(gridCellPosition);

            if (!_placedPositions.Contains(gridCellPosition))
            {

                //Create a venue and add to editor objects will be done by EditorObjectsManager
                GameObject gameObject = EditObjectsManager.AddEditorObject(gridCellPosition);
                

          

                //TODO: Quick fix we need appropiate models or implement a system
                if (gameObject != null)
                {
                    gameObject.transform.position = new Vector3(spawnPosition.x, 0, spawnPosition.y);
                    gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    gameObject.transform.localScale /= _scaleDiv;
                    //update counter position
                    StateCounter counter = gameObject.GetComponent<StateCounter>();
                    counter.InstantiateCounter(spawnPosition);

                }
                _placedPositions.Add(gridCellPosition);





    }
            else
            {
                Debug.Log("Position already used !");
                OnEditorObjectClicked?.Invoke(gridCellPosition);

            }
        }
    }
}
