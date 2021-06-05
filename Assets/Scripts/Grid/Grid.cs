using System;
using UnityEngine;

namespace Grid
{
    class Grid
    {
        public float CellSize { get; set; } = 1f;

        public Vector2Int GetGridCell(Vector2 absolutePosition)
        {
            return new Vector2Int(
                RoundNumberToNextSmallestInteger(absolutePosition.x / CellSize),
                RoundNumberToNextSmallestInteger(absolutePosition.y / CellSize)
            );
        }

        public Vector2 GetRelativeWorldPosition(Vector2Int gridCell)
        {
            return new Vector2(gridCell.x * CellSize + CellSize / 2, gridCell.y * CellSize + CellSize / 2);
        }

        /// <summary>
        /// Round floating-point number to the next smallest integer.
        /// This method however specifically also rounds negative integers to the next smallest integer.
        ///
        /// <para>
        /// For example:<br/>
        /// -0.9 to -1 instead of 0
        /// -1.7 to -2 instead of -1
        /// </para>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private int RoundNumberToNextSmallestInteger(float num)
        {
            if (num < 0)
            {
                return -Mathf.CeilToInt(-num);
            }

            return (int) num;
        }
    }
}
