using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EditorObjects;

namespace Grid
{
    /// <summary>
    /// Class which places objects to the grid.
    ///This class maintains the objects in the customized world.
    ///
    /// The Grid is currently a 10 * 10 Grid, place for 100 Objects, works perfect with a plane with a 2.5 scale
    /// 
    /// TODOS:
    /// Management of differerent Prefabs -Done
    /// Automatic calculation of originPoition and width and height using the plane transform
    /// Add graph compatibility - Done
    /// 
    /// Nice to have / Maybe needed later:
    /// Dynamic plane scalling  - still buggy at some values
    /// Place prefab on multiple grid cells and check if it can be placed
    /// Pathfinding/GridSystem as data Structure -> I think using navmesh would be a possibility without own grid system
    /// 
    /// 
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        //The GameObject prefab to spawn
        [SerializeField] private GameObject _currentPrefabToSpawn;
        public GameObject CurrentPrefabToSpawn { get => _currentPrefabToSpawn; set => _currentPrefabToSpawn = value; }

        //In this position we start to spawn our grid
        public Vector3 OriginPosition;
        //Struct to see prefabs with Name in the Inspector
        [Serializable]
        public struct NamedPrefab
        {
            //public string name;
            public PrefabName prefabName;
            public GameObject prefab;
        }
        public NamedPrefab[] Prefabs;

        private EndlessGrid _grid;
        //We use this hashset to make sure that a a object is placed only once
        private HashSet<Vector3> _placedPositions;
        [SerializeField] private Transform _planeWorldTransform;
        //To Check if we clicked the correct layer
        [SerializeField] private LayerMask _groundMask;
        //To scale the used prefabs, temporarily
        [SerializeField] private float _scaleDiv = 2f;

        private PrefabName _currentPrefabName = PrefabName.None;

        #region Debug Grid System variables
        [SerializeField] private int _height = 10;
        [SerializeField] private int _width = 10;
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private bool _drawGrid= true;
        #endregion

        private void Start()
        {
            _placedPositions = new HashSet<Vector3>();
            _grid = FindObjectOfType<EndlessGrid>();
            if (_planeWorldTransform.localScale.x != _planeWorldTransform.localScale.z) Debug.LogWarning("Plane must be a square !");
            _cellSize = _planeWorldTransform.localScale.x;
            _grid.GapSize = _cellSize;
            if(_drawGrid)
                DebugDrawGrid();
        }

        // Update is called once per frame
        private void Update()
        {
            //Check if left mouse button clicked and UI not clicked
            if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false && _currentPrefabToSpawn !=null)
            {
                //Raycast into the scene
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Check if we hit something
                if (Physics.Raycast(ray, out hitInfo,Mathf.Infinity,_groundMask))
                {
                    PlacePrefabNear(hitInfo.point);
                    Debug.Log("Relative Position On Grid: " + ConvertToRelativePosition(hitInfo.point));
                } 
            }

        }


        private void PlacePrefabNear(Vector3 clickPoint)
        {
            Vector3 spawnPosition = _grid.GetNearestPointOnGrid(clickPoint);
            if (!_placedPositions.Contains(spawnPosition))
            {


                //Here we must use the Editor Object Factory

                // Instantiate at finalPosition and zero rotation.
                //GameObject gameObject = Instantiate(_currentPrefabToSpawn, spawnPosition, Quaternion.identity);
                //gameObject.transform.parent = _planeWorldTransform;
                GameObject gameObject = null;

                //Rethink strucutre here
                Vector3 relativePosition = ConvertToRelativePosition(spawnPosition);
                Vector3Int relativePositionInt = new Vector3Int((int)relativePosition.x,(int)relativePosition.y,(int)relativePosition.z);

                //Here we may add the editor objects to an corresponding controller / manager
                switch (_currentPrefabName)
                {

                    case PrefabName.Venue:
                        gameObject = EditorObjectFactory.CreateVenueEditorObject(_currentPrefabToSpawn, spawnPosition, relativePositionInt, _planeWorldTransform).EditorGameObject;
                        break;

                    case PrefabName.Workplace:
                        gameObject = EditorObjectFactory.CreateWorkplaceEditorObject(_currentPrefabToSpawn, spawnPosition, relativePositionInt, _planeWorldTransform).EditorGameObject;
                        break;

                    case PrefabName.Hospital:
                        gameObject = EditorObjectFactory.CreateHospitalEditorObject(_currentPrefabToSpawn, spawnPosition, relativePositionInt, _planeWorldTransform).EditorGameObject;
                        break;

                    case PrefabName.Household:
                        gameObject = EditorObjectFactory.CreateHouseholdEditorObject(_currentPrefabToSpawn, spawnPosition, relativePositionInt, _planeWorldTransform).EditorGameObject;
                        break;
                    
                    default:
                        Debug.LogError("Unknown prefab name");
                        break;
                }

                //TODO: Quick fix we need appropiate models or implement a system
                if (gameObject != null)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    gameObject.transform.localScale /= _scaleDiv;
                }
                _placedPositions.Add(spawnPosition);
            }
            else Debug.Log("Position already used !");
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
                    CurrentPrefabToSpawn = namedPrefab.prefab;
                    Debug.Log("Set Prefab Name:" + namedPrefab.prefabName);
                    _currentPrefabName = namedPrefab.prefabName;
                    return;
                }
            }
        }



        #region GridSystem For Debug purposes

        /// <summary>
        /// Method to convert a relative position to an approximated world position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector3 ConvertToWorldPosition(int x, int y)
        {
            return new Vector3(x, 0, y) * _cellSize + OriginPosition;
        }

        /// <summary>
        /// Method which converts a vector representing the world position 
        /// to a relative one which can be saved in a gridArray, grid Array is
        /// is indexed from down left to upper right.
        /// 
        /// Example of a 2*2 relative Grid indexing:
        /// 
        /// [1,0] [1,1]
        /// [0,0] [0,1]
        /// 
        /// 
        /// Relative Positions can be later used to manage pathfinding/check of objects occupy multiple Grids
        /// </summary>
        /// <param name="worldPosition">The Position in the 3D-World</param>
        /// <returns></returns>
        public Vector3 ConvertToRelativePosition(Vector3 worldPosition)
        {
            Vector3 resultVector = new Vector3
            {
                x = Mathf.FloorToInt((worldPosition - OriginPosition).x / _cellSize),
                y = Mathf.FloorToInt((worldPosition - OriginPosition).z / _cellSize)
            };

            return resultVector;
        }


        /// <summary>
        /// Method to drwa a Grid for Debuggung purposes.
        /// </summary>
        private void DebugDrawGrid()
        {

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    //This places text object in each grid position
                    //Text is offsetted by the cellSize multipled with a factor
                    string relativeCoordinatesString = "(" + x + "," + y + ")";
                    CreateWorldTextMesh(relativeCoordinatesString, null, ConvertToWorldPosition(x, y) + new Vector3(_cellSize, _cellSize) * .5f, 10, Color.red, TextAnchor.MiddleCenter);

                    //To see the horizontal and vertical line (Draw gizmos must be activated)
                    Debug.DrawLine(ConvertToWorldPosition(x, y), ConvertToWorldPosition(x, y + 1), Color.red);
                    Debug.DrawLine(ConvertToWorldPosition(x, y), ConvertToWorldPosition(x + 1, y), Color.red);
                }
            }


            //Draw last two horizontal and vertical lines
            Debug.DrawLine(ConvertToWorldPosition(0, _height), ConvertToWorldPosition(_width, _height), Color.red);
            Debug.DrawLine(ConvertToWorldPosition(_width, 0), ConvertToWorldPosition(_width, _height), Color.red);
        }

    
        /// <summary>
        /// Method which creates the world text recursively.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="parent"></param>
        /// <param name="localPosition"></param>
        /// <param name="fontSize"></param>
        /// <param name="color"></param>
        /// <param name="textAnchor"></param>
        /// <param name="textAlignment"></param>
        /// <param name="sortingOrder"></param>
        /// <returns></returns>
        private static TextMesh CreateWorldTextMesh(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 5, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
        {
            if (color == null) color = Color.white;
            return CreateWorldTextMesh(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }

        // Create Text in the World
        private static TextMesh CreateWorldTextMesh(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("WorldText", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            //Place text on plane, for this reason rotation required
            transform.Rotate(new Vector3(90,0,0));
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        #endregion
    }
}