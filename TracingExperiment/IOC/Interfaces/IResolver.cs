namespace TracingExperiment.IOC.Interfaces
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}