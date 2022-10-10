using System;
using MultilayerPerseprotron.Extensions;

namespace MultilayerPerseprotron.DataStructure
{
    public class Vector : ICloneable
    {
        private readonly double[] vector;

        public Vector(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Invalid value", nameof(length));
            }

            vector = new double[length];
        }

        public Vector(params double[] items)
        {
            if (items.Length <= 0)
            {
                throw new ArgumentException("Invalid value", nameof(items));
            }

            vector = new double[items.Length];
            for (int i = 0; i < items.Length; ++i)
            {
                vector[i] = items[i];
            }
        }

        public int Length => vector.Length;

        public static Vector CreateRandom(int length)
        {
            var random = new Random();
            var result = new Vector(length);
            for (int i = 0; i < length; ++i)
            {
                result[i] = random.NextDouble(-1, 1);      
            }

            return result;
        }

        public double this[int index]
        {
            get
            {
                CheckParameters(index);
                return vector[index];
            }
            set
            {
                CheckParameters(index);
                vector[index] = value;
            }
        }

        public static Vector operator +(Vector left, Vector right)
            => Template(left, right, (l, r) => l + r);

        public static Vector operator -(Vector left, Vector right)
            => Template(left, right, (l, r) => l - r);

        public static Vector operator *(Vector left, Vector right)
            => Template(left, right, (l, r) => l * r);

        private void CheckParameters(int index)
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentException("Index out of range", nameof(index));
            }
        }

        public object Clone()
        {
            var clone = new Vector(this.Length);
            for (int i = 0; i < this.Length; ++i)
            {
                clone[i] = this[i];
            }

            return clone;
        }

        private static Vector Template(Vector left, Vector right, Func<double, double, double> func)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (left.Length != right.Length)
            {
                throw new ArgumentException("Length is invalid");
            }

            var result = new Vector(left.Length);
            for (int i = 0; i < left.Length; ++i)
            {
                result[i] = func(left[i], right[i]);
            }

            return result;
        }
    }
}
