using UnityEngine;

public static class MathC
{
    /// <summary>
    /// <para>
    /// Maps the linear parameter <paramref name="x"/> between 0 and 1 to a
    /// factor between 1/<paramref name="minMaxFactor"/> and <paramref name="minMaxFactor"/>.
    /// </para>
    /// 
    /// <para>
    /// For <paramref name="x"/> &gt; 0.5, the result will be lerped between 1 and <paramref name="minMaxFactor"/>.<br/>
    /// For <paramref name="x"/> &lt; 0.5, the result will be between 1/<paramref name="minMaxFactor"/> and 1.<br/>
    /// For <paramref name="x"/> = 0.5, the result will be exactly 1 as a neutral factor.
    /// </para>
    /// </summary>
    /// 
    /// <param name="minMaxFactor">The value defining the bounds for the resulting factor</param>
    /// <param name="x">A linear value between 0 and 1 which is supposed to be mapped to a factor based on <paramref name="minMaxFactor"/></param>
    /// <returns>A factor between 1/<paramref name="minMaxFactor"/> and <paramref name="minMaxFactor"/> based on <paramref name="x"/></returns>
    public static float MapLinearToFactor(float minMaxFactor, float x)
    {
        if (x > 0.5f)
        {
            // Maps the space between 0.5 and +infinity to the linear space between 1 and minMaxFactor
            return Mathf.Lerp(1, minMaxFactor, (x - 0.5f) * 2);
        }

        if (x < 0.5)
        {
            // Maps the space between -infinity and 0.5 to the 1/x space between 1/minMaxFactor and 1
            return 1 / Mathf.Lerp(minMaxFactor, 1, x * 2);
        }
            
        return 1;
    }
}