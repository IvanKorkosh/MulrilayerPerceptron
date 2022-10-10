using MultilayerPerseprotron.DataStructure;

namespace MultilayerPerseprotron
{
    public interface INetwork
    {
        public Vector Recognize(Vector input);

        void Learn(Vector idealResult, double learningRate, double moment);
    }
}
