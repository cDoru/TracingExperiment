namespace TracingExperiment.Tracing.Concurrent
{
    internal interface IVolatileValue<T> : IValueReader<T>, IValueWriter<T> { }
}