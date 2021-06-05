using System;

namespace Simulation.Edit
{
    [Serializable]
    public class GridCell
    {
        private uint _x;
        private uint _y;
       public GridCell(uint x, uint y)
        {
            _x = x;
            _y = y;
        }

    }
}
