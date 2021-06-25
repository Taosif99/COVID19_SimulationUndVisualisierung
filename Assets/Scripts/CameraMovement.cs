using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Simple Script which implements scene camera 
/// movement with wasd und up,down, left right buttons.
/// 
/// Good Explanation for Interpolation: http://www.faustofonseca.com/tutorial/unity-vector3-lerp-vs-vector3-slerp
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] private float cameraMovementSpeed = 5;
    //Lower = smoother, Factor used for interpolation
    [SerializeField] [Range(0, 1)] private float _smoothFactor = 0.5f;
    
    [Header("Camera Zoom")]
    [SerializeField] [Range(0, 1)] private float _defaultZoom = 0.2f;
    [SerializeField] [Range(0, 1)] private float _zoomSpeed = 0.05f;
    [SerializeField] private float _minY = 12;
    [SerializeField] private float _maxY = 40;
    [SerializeField] private float _minAngle = 50;
    [SerializeField] private float _maxAngle = 75;
    
    private Camera _mainCamera;
    private float _currentZoomFactor;

    public bool IsMouseOverUi { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        //Getting the camera component
        _mainCamera = GetComponent<Camera>();
        SetCameraZoom(_defaultZoom);
    }

    // Update is called once per frame
    private void Update()
    {
        IsMouseOverUi = IsMousePointerOverUIElement();
        ZoomCamera();
        MoveCamera();
    }

    private void ZoomCamera()
    {
        SetCameraZoom(_currentZoomFactor - Input.mouseScrollDelta.y * _zoomSpeed);
    }

    private void SetCameraZoom(float zoomFactor)
    {
        zoomFactor = Mathf.Clamp(zoomFactor, 0, 1);
        _currentZoomFactor = zoomFactor;

        float y = Mathf.Lerp(_minY, _maxY, zoomFactor);
        float angle = Mathf.Lerp(_minAngle, _maxAngle, zoomFactor);

        var cameraTransform = _mainCamera.transform;
        var position = cameraTransform.position;
        cameraTransform.position = new Vector3(position.x, y, position.z);

        var eulerAngles = cameraTransform.rotation.eulerAngles;
        _mainCamera.transform.rotation = Quaternion.Euler(angle, eulerAngles.y, eulerAngles.z);
    }

    /// <summary>
    /// Method which moves the main Camera in the world.
    /// </summary>
    private void MoveCamera()
    {
        if (EventSystem.current.currentSelectedGameObject != null &&
            EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null)
        {
            return;
        }
        
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0,
            Input.GetAxis("Vertical"));
        Vector3 targetPosition = _mainCamera.transform.position + inputVector * Time.deltaTime * cameraMovementSpeed;
        _mainCamera.transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothFactor);
    }
 
    /// <summary>
    /// Method to check if mouse points to main UI elements.
    /// </summary>
    /// <returns>true if main UI elements are hit, else false.</returns>
    private static bool IsMousePointerOverUIElement()
    {
        //Getting event system raycast results
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raysastResults);
        //Checking if UI Element hit
        for (int i = 0; i < raysastResults.Count; i++)
        {
            RaycastResult curRaysastResult = raysastResults[i];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }
}