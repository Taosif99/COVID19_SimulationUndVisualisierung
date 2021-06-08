using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Simple Script which implements scene camera 
/// movement with wasd und up,down, left right buttons.
/// 
/// Good Explanation for Interpolation: http://www.faustofonseca.com/tutorial/unity-vector3-lerp-vs-vector3-slerp
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float cameraMovementSpeed = 5;
    //Lower = smoother, Factor used for interpolation
    [SerializeField] private float _smoothFactor = 0.5f;
    private bool _isMouseOverUi;
    public bool IsMouseOverUi { get => _isMouseOverUi; set => _isMouseOverUi = value; }



    // Start is called before the first frame update
    private void Start()
    {
        //Getting the camera component
        _mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        IsMouseOverUi = IsMousePointerOverUIElement();
        MoveCamera();
    }

    /// <summary>
    /// Method which moves the main Camera in the world.
    /// </summary>
    private void MoveCamera()
    {
        //Check if 2D UI is not hit by the mouse
       if(!_isMouseOverUi)
        {

            Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0,
                                                Input.GetAxis("Vertical"));
            Vector3 targetPosition = _mainCamera.transform.position + inputVector * Time.deltaTime * cameraMovementSpeed;
            _mainCamera.transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothFactor);

        }   
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