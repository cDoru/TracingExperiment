namespace TracingExperiment.Tracing.Concurrent
{
    internal interface IValueReader<out T>
    {
        T GetValue();
        T NonVolatileGetValue();
    }
}