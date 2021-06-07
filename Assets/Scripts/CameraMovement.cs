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

    //TODO FIX 3D CANVAS STOPPING, since a canvas cannot be raycasted currently


    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float cameraMovementSpeed = 5;
    //Lower = smoother, Factor used for interpolation
    [SerializeField] private float _smoothFactor = 0.5f;
    [SerializeField] private LayerMask _worldUIMask;

    // Start is called before the first frame update
    private void Start()
    {
        //Getting the camera component
        _mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        MoveCamera();
    }

    /// <summary>
    /// Method which moves the main Camera in the world.
    /// </summary>
    private void MoveCamera()
    {
        //Better later implement an inputHandler / manager

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hits3DUI = Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _worldUIMask);
        Debug.Log("Hits 3d ui ?" + hits3DUI);
        if (EventSystem.current.IsPointerOverGameObject() == false || hits3DUI)
        {

            Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0,
                                                Input.GetAxis("Vertical"));
            Vector3 targetPosition = _mainCamera.transform.position + inputVector * Time.deltaTime * cameraMovementSpeed;
            _mainCamera.transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothFactor);

        }
        
        }

}