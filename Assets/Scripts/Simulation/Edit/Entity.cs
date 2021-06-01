using System;

namespace Simulation.Edit
{
    [Serializable]
    public class Entity
    {
        private GridCell _position;
        public Entity(GridCell position)
        {
            _position = position;
        }
    }
}