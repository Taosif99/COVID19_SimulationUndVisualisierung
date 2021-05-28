using System;

namespace Data
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