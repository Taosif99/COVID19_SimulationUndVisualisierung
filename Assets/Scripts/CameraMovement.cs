using UnityEngine;

/// <summary>
/// Simple Script which implements scene camera 
/// movement with wasd und up,down, left right buttons.
/// </summary>
public class CameraMovement : MonoBehaviour
{

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float cameraMovementSpeed = 5;


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
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"),0,
                                            Input.GetAxis("Vertical"));
        _mainCamera.transform.position += inputVector * Time.deltaTime * cameraMovementSpeed;
    }
}
