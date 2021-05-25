using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    [SerializeField]
    GameObject objectToDestroy;
   

    public void DestroyGameObject()
    {
        Destroy(objectToDestroy);
        Destroy(this.gameObject);

    }

    
}
