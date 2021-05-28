using System;

namespace Data
{
    [Serializable]
    public class Vector2
    {
        private float _x;
        private float _y;
       public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }
    }
}
