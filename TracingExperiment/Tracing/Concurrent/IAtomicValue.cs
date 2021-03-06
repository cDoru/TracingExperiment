namespace TracingExperiment.Tracing.Concurrent
{
    internal interface IAtomicValue<T> : IVolatileValue<T>
    {
        T Add(T value);

        T GetAndAdd(T value);
        T GetAndIncrement();
        T GetAndIncrement(T value);
        T GetAndDecrement();
        T GetAndDecrement(T value);

        T Increment();
        T Increment(T value);
        T Decrement();
        T Decrement(T value);

        T GetAndReset();
        T GetAndSet(T newValue);
        bool CompareAndSwap(T expected, T updated);
    }
}