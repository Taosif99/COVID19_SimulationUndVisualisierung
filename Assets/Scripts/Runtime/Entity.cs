using UnityEngine;

namespace Assets.Scripts.Runtime
{
    class Entity : MonoBehaviour
    {
        protected void Initialize(Vector2 position)
        {
            gameObject.transform.position = new Vector3(position.x, 0, position.y);
        }
    }
}
