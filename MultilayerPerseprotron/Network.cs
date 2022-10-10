using System;
using MultilayerPerseprotron.Activators;
using MultilayerPerseprotron.DataStructure;

namespace MultilayerPerseprotron
{
    public class Network : INetwork
    {
        private readonly IActivator activator;
        private readonly Matrix inputLayerWeights;
        private readonly Matrix hiddenLayerWeights;

        private readonly Matrix previousInputDeltaWeight;
        private readonly Matrix previousHiddenDeltaWeight;

        private readonly Vector inputLayerOffsetWeight;
        private readonly Vector hiddenLayerOffsetWeight;

        private Vector inputLayerOutput;
        private Vector hiddenLayerOutput;
        private Vector outputLayerOutput;

        public Network(IActivator activator, int inputNeuronsNumber, int hiddenNeuronsNumber, int outputNeuronsNumber)
        {
            if (activator is null)
            {
                throw new ArgumentNullException(nameof(activator));
            }

            this.activator = activator;

            inputLayerWeights = Matrix.CreateRandom(hiddenNeuronsNumber, inputNeuronsNumber);
            hiddenLayerWeights = Matrix.CreateRandom(outputNeuronsNumber, hiddenNeuronsNumber);

            previousInputDeltaWeight = new Matrix(hiddenNeuronsNumber, inputNeuronsNumber);
            previousHiddenDeltaWeight = new Matrix(outputNeuronsNumber, hiddenNeuronsNumber);

            inputLayerOffsetWeight = Vector.CreateRandom(hiddenNeuronsNumber);
            hiddenLayerOffsetWeight = Vector.CreateRandom(outputNeuronsNumber);
        }

        public Vector Recognize(Vector input)
        {
            inputLayerOutput = (Vector)input.Clone();
            hiddenLayerOutput = activator.Process((inputLayerWeights * input) + inputLayerOffsetWeight);
            outputLayerOutput = activator.Process((hiddenLayerWeights * hiddenLayerOutput) + hiddenLayerOffsetWeight);
            return (Vector)outputLayerOutput.Clone();
        }

        public void Learn(Vector idealResult, double learningRate, double moment)
        {
            if (outputLayerOutput.Length != idealResult.Length)
            {
                throw new ArgumentException("Invalid length of vectors");
            }

            var outputLayerDelta = (idealResult - outputLayerOutput) * activator.ProcessDiff(outputLayerOutput);
            var hiddenLayerDelta = Matrix.GetTransposed(hiddenLayerWeights) * outputLayerDelta * activator.ProcessDiff(hiddenLayerOutput);

            UpdateMatrix(inputLayerWeights, previousInputDeltaWeight, inputLayerOutput, hiddenLayerDelta, learningRate, moment);
            UpdateMatrix(hiddenLayerWeights, previousHiddenDeltaWeight, hiddenLayerOutput, outputLayerDelta, learningRate, moment);

            UpdateVector(inputLayerOffsetWeight, hiddenLayerDelta, learningRate);
            UpdateVector(hiddenLayerOffsetWeight, outputLayerDelta, learningRate);
        }

        private void UpdateMatrix(Matrix weightMatrix, Matrix previousWeightDelta, Vector layerOutput, Vector delta, double learningRate, double moment)
        {
            for (int row = 0; row < weightMatrix.Height; ++row)
            {
                for (int column = 0; column < weightMatrix.Width; ++column)
                {
                    var gradient = layerOutput[column] * delta[row];
                    var deltaWeight = learningRate * gradient + moment * previousWeightDelta[row, column];
                    weightMatrix[row, column] += deltaWeight;
                    previousWeightDelta[row, column] = deltaWeight;
                }
            }
        }

        private void UpdateVector(Vector vector, Vector delta, double learningRate)
        {
            for (int i = 0; i < vector.Length; ++i)
            {
                vector[i] += delta[i] * learningRate;
            }
        }
    }
}