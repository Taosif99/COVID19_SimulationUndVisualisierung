using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [RequireComponent(typeof(GridManager))]
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField]
        private Material _material;
        
        private GridManager _gridManager;
        private int _cellExtent;
        
        private HashSet<GameObject> _gameObjects = new HashSet<GameObject>();

        private void Awake()
        {
            _gridManager = GetComponent<GridManager>();
        }

        private void Update()
        {
            if (_gridManager.CellExtent != _cellExtent)
            {
                _cellExtent = _gridManager.CellExtent;
                UpdateVisualization();                
            }
        }

        private void UpdateVisualization()
        {
            foreach (GameObject obj in _gameObjects)
            {
                Destroy(obj);
            }
            
            _gameObjects.Clear();
            
            float size = _gridManager.Grid.CellSize - _gridManager.Grid.CellSize / 5;
            for (int x = -_gridManager.CellExtent; x < _gridManager.CellExtent; x++)
            {
                for (int y = -_gridManager.CellExtent; y < _gridManager.CellExtent; y++)
                {
                    Vector2 position = _gridManager.Grid.GetRelativeWorldPosition(new Vector2Int(x, y));

                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.localScale = new Vector3(size, 0.1f, size);
                    cube.transform.localPosition = new Vector3(position.x, 0.05f, position.y);
                    cube.transform.SetParent(transform);

                    cube.GetComponent<Renderer>().sharedMaterial = _material;

                    _gameObjects.Add(cube);
                }
            }
        }
    }
}