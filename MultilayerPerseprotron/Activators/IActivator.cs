using MultilayerPerseprotron.DataStructure;

namespace MultilayerPerseprotron.Activators
{
    public interface IActivator
    {
        double Process(double x);

        Vector Process(Vector vector);

        double ProcessDiff(double x);

        Vector ProcessDiff(Vector vector);
    }
}
