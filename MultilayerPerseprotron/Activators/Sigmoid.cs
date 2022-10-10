using System;
using MultilayerPerseprotron.DataStructure;

namespace MultilayerPerseprotron.Activators
{
    public class Sigmoid : IActivator
    {
		public double Process(double x)
			=> 1 / (1 + Math.Exp(-x));

		public Vector Process(Vector vector)
			=> Template(vector, Process);

		public double ProcessDiff(double x)
			=> (1 - x) * x;

		public Vector ProcessDiff(Vector vector)
			=> Template(vector, ProcessDiff);

		private Vector Template(Vector vector, Func<double, double> func)
		{
			if (vector is null)
			{
				throw new ArgumentNullException(nameof(vector));
			}

			var result = new Vector(vector.Length);
			for (int i = 0; i < vector.Length; ++i)
			{
				result[i] = func(vector[i]);
			}

			return result;
		}		
	}
}