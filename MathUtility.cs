using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PokeItems
{
    internal class MathUtility
    {
        public static float GetLinearStacking(float value, float count, float procCoefficient = 1f)
        {
            return value * count * procCoefficient;
        }

        public static float GetLinearWithExtraStacking(float value, float extraValue, float count, float procCoefficient = 1f)
        {
            if (count <= 1)
                return GetLinearStacking(value, count, procCoefficient);

            return (value + extraValue * (count - 1)) * procCoefficient;
        }

        public static float GetExponentialPercentReductionStacking(float percent, float count)
        {
            return Mathf.Pow(1f - (percent / 100f), count - 1);
        }
    }
}