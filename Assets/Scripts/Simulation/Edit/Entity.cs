using System;

namespace Simulation.Edit
{
    [Serializable]
    public abstract class Entity
    {
        public GridCell Position { get; set; }
        protected Entity(GridCell position)
        {
            Position = position;
        }
    }
}