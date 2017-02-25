using System;
using System.Threading;
using TracingExperiment.Exceptions;

namespace TracingExperiment.Tracing.Utils
{
    public class TimingOutLock : IDisposable
    {
        private const string LockAcquisitionFailureMessage = "Failed to acquire lock (timout: {0}s)";
        private readonly object _lockObj;

        public TimingOutLock(object lockObj)
            : this(lockObj, TimeSpan.FromSeconds(3))
        {

        }

        private TimingOutLock(object lockObj, TimeSpan timeout)
        {
            _lockObj = lockObj;
            if (!Monitor.TryEnter(_lockObj, timeout))
                throw new LockingException(LockAcquisitionFailureMessage, timeout.TotalSeconds);
        }

        public void Dispose()
        {
            Monitor.Exit(_lockObj);
        }
    }
}