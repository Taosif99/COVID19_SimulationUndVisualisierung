using UnityEngine;
using UnityEngine.EventSystems;

namespace Grid
{
    /// <summary>
    /// Class which implements debugging functions for our grid implementation.
    /// </summary>
    [RequireComponent(typeof(GridManager2))]
    class GridManagerDebugger : MonoBehaviour
    {
#if UNITY_EDITOR
        private GridManager2 _gridManager;
        private Grid _grid;

        private void Start()
        {
            _gridManager = GetComponent<GridManager2>();
            _grid = _gridManager.Grid;
        }

        private void Update()
        {

        }

        private void OnDrawGizmos()
        {
            if (_grid == null)
            {
                return;
            }

            for (int x = -_gridManager.CellExtent; x < _gridManager.CellExtent; x++)
            {
                for (int y = -_gridManager.CellExtent; y < _gridManager.CellExtent; y++)
                {
                    if (x >= 0 ^ y >= 0)
                    {
                        Gizmos.color = Color.blue;
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                    }

                    Vector2 relativeWorldPosition = _grid.GetRelativeWorldPosition(new Vector2Int(x, y));
                    Vector3 worldPosition = new Vector3(relativeWorldPosition.x, 0, relativeWorldPosition.y);

                    Gizmos.DrawWireCube(worldPosition, new Vector3(_grid.CellSize, 0, _grid.CellSize));

                    UnityEditor.Handles.color = Color.black;
                    UnityEditor.Handles.Label( worldPosition, $"({x}, {y})", new GUIStyle
                    {
                        alignment = TextAnchor.UpperCenter,
                        fontSize = 10
                    });
                }
            }
        }
#endif
    }
}
