﻿using System;
using UnityEngine;

namespace Simulation.Edit
{
    [Serializable]
    public class GridCell
    {
        private int _x;
        private int _y;
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        
        public GridCell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is GridCell cell &&
                   X == cell.X &&
                   Y == cell.Y;
        }

        //Generated by Visual Studio IDE to make Hashtable function(s) work properly
        public override int GetHashCode()
        {
            int hashCode = 979593255;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public Vector2Int ToVector2Int() => new Vector2Int(X, Y);
    }
}
