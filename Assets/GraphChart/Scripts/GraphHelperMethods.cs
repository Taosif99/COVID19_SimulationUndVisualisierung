using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class shall maintain the helper methods used in the Graph implementation
/// </summary>
public static class GraphHelperMethods
{

    /// <summary>
    /// Method which returns the angle of an Vector which represents a direction.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static float GetAngleFromVector(Vector3 direction)
    {
        direction = direction.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //if (angle < 0) angle += 360; //Diese line macht bei Atan2 keinen sinn !?
        return angle;
    }

    /// <summary>
    /// Method to get the Lowest and highest integer values in multiple lists.
    /// </summary>
    /// <param name="valueLists"></param>
    /// <param name="min">The returned minimum value.</param>
    /// <param name="max">The returned maximum value.</param>
    public static void MultiIntegerListSearch(List<List<int>> valueLists, out float min, out float max)
    {
        // Identify y Min and Max values
        max = valueLists[0][0];
        min = valueLists[0][0];

        //Linear Search for each list
        foreach (List<int> valueList in valueLists)
        {
            for (int i = 0; i < valueList.Count; i++)
            {
                int value = valueList[i];
                if (value > max)
                {
                    max = value;
                }
                if (value < min)
                {
                    min = value;
                }
            }

        }

    }
}
