using System;
using MultilayerPerseprotron.Extensions;

namespace MultilayerPerseprotron.DataStructure
{
    public class Matrix
    {
        private readonly double[] matrix;

        public Matrix(int height, int width)
        {
            if (height <= 0)
            {
                throw new ArgumentException("Invalid value", nameof(height));
            }

            if (width <= 0)
            {
                throw new ArgumentException("Invalid value", nameof(width));
            }

            Height = height;
            Width = width;
            matrix = new double[height * width];
        }

        public int Height { get; }

        public int Width { get; }

        public double this[int row, int column]
        {
            get
            {
                CheckParameters(row, column);
                return matrix[row * Width + column];
            }
            set
            {
                CheckParameters(row, column);
                matrix[row * Width + column] = value;
            }
        }

        public static Matrix GetTransposed(Matrix matrix)
        {
            if (matrix is null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            var result = new Matrix(matrix.Width, matrix.Height);
            for (int row = 0; row < matrix.Height; ++row)
            {
                for (int column = 0; column < matrix.Width; ++column)
                {
                    result[column, row] = matrix[row, column];
                }
            }

            return result;
        }

        public static Vector operator*(Matrix matrix, Vector vector)
        {
            if (matrix is null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector));
            }

            if (matrix.Width != vector.Length)
            {
                throw new ArgumentException("Length is invalid");
            }

            var result = new Vector(matrix.Height);
            for (int row = 0; row < matrix.Height; ++row)
            {
                double component = 0;
                for (int column = 0; column < matrix.Width; ++column)
                {
                    component += matrix[row, column] * vector[column];   
                }

                result[row] = component;
            }

            return result;
        }

        public static Matrix CreateRandom(int height, int width)
        {
            var random = new Random();
            var result = new Matrix(height, width);
            for (int row = 0; row < height; ++row)
            {
                for (int column = 0; column < width; ++column)
                {
                    result[row, column] = random.NextDouble(-1, 1);
                }
            }

            return result;
        }

        private void CheckParameters(int row, int column)
        {
            if (row < 0 || row >= Height)
            {
                throw new ArgumentException("Index out of range", nameof(row));
            }

            if (column < 0 || column >= Width)
            {
                throw new ArgumentException("Index out of range", nameof(column));
            }
        }
    }
}
