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




    /// <summary>
    /// Quantile function (Inverse CDF) for the normal distribution
    /// reimplmeneted according the R-Function qnorm()
    /// 
    /// <see cref="https://svn.r-project.org/R/trunk/src/nmath/qnorm.c"/> 
    /// 
    /// Mainly taken from:
    /// <see cref="https://stackoverflow.com/questions/12647539/how-do-i-transform-an-inverse-cdf-coded-for-a-standard-deviation-of-one-to-a-dif"/>
    /// 
    /// Differences: We using floats to provide faster calculations (but less accuracy)
    /// 
    /// IN FUTURE VERSIONS: USE APPROPIATE LIBRARY OR EXECURE R FROM C# with R.Net
    /// 
    /// 
    /// </summary>
    /// <param name="p">Probability.</param>
    /// <param name="mu">Mean of normal distribution.</param>
    /// <param name="sigma">Standard deviation of normal distribution.</param>
    /// <param name="isLowerTail">If true, probability is P[X <= x], otherwise P[X > x].</param>
    /// <param name="isLogValues">If true, probabilities are given as log(p).</param>
    /// <returns>P[X <= x] where x ~ N(mu,sigma^2)</returns>
    public static float QNorm(float p, float mu, float sigma, bool isLowerTail, bool isLogValues)
    {
        if (float.IsNaN(p) || float.IsNaN(mu) || float.IsNaN(sigma)) return (p + mu + sigma);
        float ans;
        bool isBoundaryCase = R_Q_P01_boundaries(p, float.NegativeInfinity, float.PositiveInfinity, isLowerTail, isLogValues, out ans);
        if (isBoundaryCase) return (ans);
        if (sigma < 0) return (float.NaN);
        if (sigma == 0) return (mu);

        float p_ = R_DT_qIv(p, isLowerTail, isLogValues);
        float q = p_ - 0.5f;
        float r, val;

        if (Mathf.Abs(q) <= 0.425)  // 0.075 <= p <= 0.925
        {
            r = .180625f - q * q;
            val = q * (((((((r * 2509.0809287301226727f +
                       33430.575583588128105f) * r + 67265.770927008700853f) * r +
                     45921.953931549871457f) * r + 13731.693765509461125f) * r +
                   1971.5909503065514427f) * r + 133.14166789178437745f) * r +
                 3.387132872796366608f)
            / (((((((r * 5226.495278852854561f +
                     28729.085735721942674f) * r + 39307.89580009271061f) * r +
                   21213.794301586595867f) * r + 5394.1960214247511077f) * r +
                 687.1870074920579083f) * r + 42.313330701600911252f) * r + 1.0f);
        }
        else
        {
            r = q > 0 ? R_DT_CIv(p, isLowerTail, isLogValues) : p_;
            r = Mathf.Sqrt(-((isLogValues && ((isLowerTail && q <= 0) || (!isLowerTail && q > 0))) ? p : Mathf.Log(r)));

            if (r <= 5)
            {
                r -= 1.6f;
                val = (((((((r * 7.7454501427834140764e-4f +
                        .0227238449892691845833f) * r + .24178072517745061177f) *
                      r + 1.27045825245236838258f) * r +
                     3.64784832476320460504f) * r + 5.7694972214606914055f) *
                   r + 4.6303378461565452959f) * r +
                  1.42343711074968357734f)
                 / (((((((r *
                          1.05075007164441684324e-9f + 5.475938084995344946e-4f) *
                         r + .0151986665636164571966f) * r +
                        .14810397642748007459f) * r + .68976733498510000455f) *
                      r + 1.6763848301838038494f) * r +
                     2.05319162663775882187f) * r + 1.0f);
            }
            else
            {
                r -= 5.0f;
                val = (((((((r * 2.01033439929228813265e-7f +
                        2.71155556874348757815e-5f) * r +
                       .0012426609473880784386f) * r + .026532189526576123093f) *
                     r + .29656057182850489123f) * r +
                    1.7848265399172913358f) * r + 5.4637849111641143699f) *
                  r + 6.6579046435011037772f)
                 / (((((((r *
                          2.04426310338993978564e-15f + 1.4215117583164458887e-7f) *
                         r + 1.8463183175100546818e-5f) * r +
                        7.868691311456132591e-4f) * r + .0148753612908506148525f)
                      * r + .13692988092273580531f) * r +
                     .59983220655588793769f) * r + 1.0f);
            }
            if (q < 0.0) val = -val;
        }

        return (mu + sigma * val);
    }

    private static bool R_Q_P01_boundaries(float p, float left, float right, bool isLowerTail, bool isLogValues, out float ans)
    {
        if (isLogValues)
        {
            if (p > 0.0)
            {
                ans = float.NaN;
                return (true);
            }
            if (p == 0.0)
            {
                ans = isLowerTail ? right : left;
                return (true);
            }
            if (p == float.NegativeInfinity)
            {
                ans = isLowerTail ? left : right;
                return (true);
            }
        }
        else
        {
            if (p < 0.0 || p > 1.0)
            {
                ans = float.NaN;
                return (true);
            }
            if (p == 0.0)
            {
                ans = isLowerTail ? left : right;
                return (true);
            }
            if (p == 1.0)
            {
                ans = isLowerTail ? right : left;
                return (true);
            }
        }
        ans = float.NaN;
        return (false);
    }

    private static float R_DT_qIv(float p, bool isLowerTail, bool isLogValues)
    {
        return (float)(isLogValues ? (isLowerTail ? Mathf.Exp(p) : -ExpM1(p)) : R_D_Lval(p, isLowerTail));
    }

    private static float R_DT_CIv(float p, bool isLowerTail, bool isLogValues)
    {
        return (float)(isLogValues ? (isLowerTail ? -ExpM1(p) : Mathf.Exp(p)) : R_D_Cval(p, isLowerTail));
    }

    private static float R_D_Lval(float p, bool isLowerTail)
    {
        return isLowerTail ? p : 0.5f - p + 0.5f;
    }

    private static float R_D_Cval(float p, bool isLowerTail)
    {
        return isLowerTail ? 0.5f - p + 0.5f : p;
    }
    private static float ExpM1(float x)
    {
        if (Mathf.Abs(x) < 1e-5)
            return x + 0.5f * x * x;
        else
            return Mathf.Exp(x) - 1.0f;
    }





}