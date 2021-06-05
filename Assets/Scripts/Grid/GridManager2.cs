using UnityEngine;

namespace Grid
{
    [RequireComponent(typeof(MeshFilter))]
    class GridManager2 : MonoBehaviour
    {
        [SerializeField] private int _cellExtent = 10;
        private Mesh _plane;

        public Grid Grid { get; private set; }
        public int CellExtent => _cellExtent;

        private void Awake()
        {
            _plane = GetComponent<MeshFilter>().mesh;
            Grid = new Grid
            {
                CellSize = _plane.bounds.extents.x * transform.localScale.x / CellExtent
            };

            Debug.Log("Using cell size " + Grid.CellSize);
        }
    }
}
