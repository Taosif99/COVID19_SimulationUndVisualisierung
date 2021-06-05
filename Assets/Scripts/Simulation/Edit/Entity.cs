using System;

namespace Simulation.Edit
{
    [Serializable]
    public abstract class Entity
    {
        private GridCell _position;
        protected Entity(GridCell position)
        {
            _position = position;
        }
    }
}