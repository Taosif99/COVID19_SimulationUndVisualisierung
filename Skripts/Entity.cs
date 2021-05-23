using System;

namespace KLASSEN_INF21
{
    [Serializable]
    public class Entity
    {
        private Vector2 _position;
        public Entity(Vector2 position)
        {
            _position = position;
        }
    }
}