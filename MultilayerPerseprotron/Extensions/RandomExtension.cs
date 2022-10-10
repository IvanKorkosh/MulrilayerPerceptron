using System;

namespace MultilayerPerseprotron.Extensions
{
    internal static class RandomExtension
    {
        static public double NextDouble(this Random random, double minValue, double maxValue)
            => random.NextDouble() * (maxValue - minValue) + minValue;
    }
}
