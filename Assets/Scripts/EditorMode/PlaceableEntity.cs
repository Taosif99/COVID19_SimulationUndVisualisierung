using System;
using EditorObjects;
using Grid;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class PlaceableEntity : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private GridManager _gridManager;
    
    [SerializeField]
    private PrefabName _prefabName;
    
    [SerializeField]
    private float _entityScale = 0.5f;
    
    private LayerMask _groundLayer;
    private GameObject _prefab;
    private GameObject _entityObject;
    private Vector2Int _currentGridCell;

    public PrefabName PrefabName => _prefabName;

    private void Start()
    {
        _groundLayer = LayerMask.GetMask("Ground");
        _prefab = ModelSelector.Instance.GetPrefab(PrefabName);
        
        Assert.IsNotNull(_gridManager);
        Assert.IsNotNull(_prefab, $"Prefab {PrefabName} not found");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _entityObject = Instantiate(_prefab);
        
        foreach (Material material in _entityObject.GetComponent<Renderer>().materials)
        {
            material.color = new Color(material.color.r, material.color.g, material.color.b, 0.5f);
        }
        
        // TODO: Don't hard-code like this
        _entityObject.transform.localScale *= _entityScale;
        _entityObject.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_entityObject);
        _entityObject = null;

        if (_gridManager.CameraController.IsMouseOverUi)
        {
            return;
        }
        
        if (CanPlaceAtCurrentGridCell())
        {
            GameObject gameObject = _gridManager.EditObjectsManager.AddEditorObject(PrefabName, _currentGridCell);
            _gridManager.PositionObjectInGrid(gameObject, _currentGridCell);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Camera.main == null)
        {
            Debug.LogWarning("Camera.main is null");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, float.PositiveInfinity, _groundLayer))
        {
            return;
        }

        var worldPoint = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
        Vector3 localPoint = worldPoint - _gridManager.transform.position;
        
        _currentGridCell = _gridManager.Grid.GetGridCell(new Vector2(localPoint.x, localPoint.z));

        if (CanPlaceAtCurrentGridCell())
        {
            Vector2 relativeWorldPosition = _gridManager.Grid.GetRelativeWorldPosition(_currentGridCell);
            _entityObject.transform.position = new Vector3(relativeWorldPosition.x, 0, relativeWorldPosition.y) +
                                               _gridManager.transform.position;
        }
        else
        {
            _entityObject.transform.position = worldPoint;
        }
    }

    private bool CanPlaceAtCurrentGridCell()
    {
        return !_gridManager.PlacedPositions.Contains(_currentGridCell)
               && Math.Abs(_currentGridCell.x) <= _gridManager.CellExtent
               && Math.Abs(_currentGridCell.y) <= _gridManager.CellExtent;
    }
}
