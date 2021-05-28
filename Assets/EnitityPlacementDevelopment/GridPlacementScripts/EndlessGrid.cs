using UnityEngine;



/// <summary>
/// This class maintains the current position of the Grid.
///The size of the grid is only restricted by the world plane/ hits possible to the plane.
///Thus it can be endless.This class maintains the current position of the Grid.
///The size of the grid is only restricted by the world plane/ hits possible to the plane.
//Thus it can be endless.
/// </summary>
public class EndlessGrid : MonoBehaviour
{

    //The gap between the points
    [SerializeField] private float _gapSize = 1f;
    public float GapSize { get => _gapSize; set => _gapSize = value; }

    //Variables for Debug purposes
    [SerializeField] private bool _drawGizmos = false;
    [SerializeField] private int _amountDebugGizmoPoints = 20;

  
    /// <summary>
    /// Method to get the nearest position in the grid the user clicked to.
    /// </summary>
    /// <param name="position">The position the user clicked to.</param>
    /// <returns></returns>
    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        //We substract and add the transform.position to get the offset working if grid moves !
        position -= transform.position;
        int xCount = Mathf.RoundToInt(position.x / _gapSize);
        int yCount = Mathf.RoundToInt(position.y / _gapSize);
        int zCount = Mathf.RoundToInt(position.z / _gapSize);
        Vector3 result = new Vector3((float)xCount, (float)yCount, (float)zCount) * _gapSize;
        result += transform.position;
        return result;
    }


    //Drawing the points debug, can be removed later, drops FPS massively
    private void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            Gizmos.color = Color.yellow;
            for (float x = 0; x < _amountDebugGizmoPoints; x += GapSize)
            {
                for (float z = 0; z < _amountDebugGizmoPoints; z += GapSize)
                {
                    var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                    //Debug.Log("Draw point:" +  point);
                    Gizmos.DrawSphere(point, 0.1f); // radius = 0.1
                }
            }
        }
    }
}
