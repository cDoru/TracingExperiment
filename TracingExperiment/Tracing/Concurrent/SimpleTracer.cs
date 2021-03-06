using System;
using System.Collections.Generic;
using System.Linq;
using TracingExperiment.Tracing.Database;
using TracingExperiment.Tracing.Interfaces;
using TracingExperiment.Tracing.Utils;
using TracingExperiment.Tracing.Utils.Interfaces;

namespace TracingExperiment.Tracing.Concurrent
{
    public class SimpleTracer : ITracer
    {
        private readonly INow _now;
        private readonly IHelper _helper;
        private readonly object _thisLock = new Object();
        private ConcurrentList<TraceStep> _steps;
        private AtomicInteger _index;

        public SimpleTracer(INow now, IHelper helper)
        {
            _now = now;
            _helper = helper;
            using (new TimingOutLock(_thisLock))
            {
                _steps = new ConcurrentList<TraceStep>();
                _index = new AtomicInteger(0);
                _index.Increment();
            }
        }

        public void WriteMessage(string source, string frame, string message)
        {
            if (_helper.ShouldLog)
            {
                using (new TimingOutLock(_thisLock))
                {
                    var index = _index.GetValue();

                    var step = new TraceStep
                    {
                        Index = index,
                        Message = message,
                        Frame = frame,
                        Source = source,
                        StepTimestamp = _now.UtcNow,
                        Metadata = string.Empty,
                        Type = StepType.Message,
                        Name = string.Empty
                    };

                    _steps.Add(step);

                    _index.Increment();
                }
            }
        }

        public void WriteException(string source, string frame, string exception, string description, string name)
        {
            if (_helper.ShouldLog)
            {
                using (new TimingOutLock(_thisLock))
                {
                    var index = _index.GetValue();

                    var step = new TraceStep
                    {
                        Name = name,
                        Index = index,
                        Frame = frame,
                        Source = source,
                        Metadata = exception,
                        Type = StepType.Exception,
                        StepTimestamp = _now.UtcNow,
                        Message = description
                    };

                    _steps.Add(step);

                    _index.Increment();
                }
            }
        }

        public void WriteOperation(string source, string frame, string description, string name, string operationMetadata)
        {
            if (_helper.ShouldLog)
            {
                using (new TimingOutLock(_thisLock))
                {
                    var index = _index.GetValue();

                    var step = new TraceStep
                    {
                        Index = index,
                        Frame = frame,
                        Source = source,
                        Metadata = operationMetadata,
                        Type = StepType.Operation,
                        StepTimestamp = _now.UtcNow,
                        Message = description,
                        Name = name
                    };

                    _steps.Add(step);

                    _index.Increment();
                }
            }
        }

        

        public List<TraceStep> TraceSteps
        {
            get
            {
                var steps = !_helper.ShouldLog ? new List<TraceStep>() : _steps.OrderBy(x => x.Index).ToList();
                return steps;
            }
        }

        public void Clear()
        {
            using (new TimingOutLock(_thisLock))
            {
                _steps = new ConcurrentList<TraceStep>();
                _index = new AtomicInteger(0);
                _index.Increment();
            }
        }
    }
}