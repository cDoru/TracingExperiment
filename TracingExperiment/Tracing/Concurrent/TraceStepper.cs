using System;
using System.Diagnostics;
using ProductionStackTrace;
using TracingExperiment.Tracing.Interfaces;
using TracingExperiment.Tracing.Utils;

namespace TracingExperiment.Tracing.Concurrent
{
    public class TraceStepper : ITraceStepper
    {
        private readonly ITracer _tracer;
        public TraceStepper(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void WriteMessage(string message)
        {
            var stackTrace = new StackTrace();

            var stackTraceInfo = StackTraceParser.Parse(
                stackTrace.ToString(),
                (f, t, m, pl, ps, fn, ln) => new
                {
                    Frame = f,
                    Type = t,
                    Method = m,
                    ParameterList = pl,
                    Parameters = ps,
                    File = fn,
                    Line = ln,
                });


            string methodName = null;
            string frame = null;

            bool @break = false;
            foreach (var stackTraceEntry in stackTraceInfo)
            {
                if (@break)
                {
                    methodName = stackTraceEntry.Method;
                    frame = stackTraceEntry.Frame;

                    break;
                }

                var type = stackTraceEntry.Type;
                if (string.IsNullOrEmpty(type)) continue;

                var @typeof = typeof(TraceStepper).Name;
                if (type.EndsWith(@typeof))
                {
                    @break = true;
                }
            }


            _tracer.WriteMessage(methodName, frame, message);
        }

        public void WriteException(Exception exception, string description)
        {
            var stackTrace = new StackTrace();

            var stackTraceInfo = StackTraceParser.Parse(
                stackTrace.ToString(),
                (f, t, m, pl, ps, fn, ln) => new
                {
                    Frame = f,
                    Type = t,
                    Method = m,
                    ParameterList = pl,
                    Parameters = ps,
                    File = fn,
                    Line = ln,
                });
            string methodName = null;
            string frame = null;

            bool @break = false;
            foreach (var stackTraceEntry in stackTraceInfo)
            {
                if (@break)
                {
                    methodName = stackTraceEntry.Method;
                    frame = stackTraceEntry.Frame;

                    break;
                }

                var type = stackTraceEntry.Type;
                if (string.IsNullOrEmpty(type)) continue;

                var @typeof = typeof(TraceStepper).Name;
                if (type.EndsWith(@typeof))
                {
                    @break = true;
                }
            }

            var exceptionName = exception.GetType().Name;
            var exceptionTrace = ExceptionReporting.GetExceptionReport(exception);
            _tracer.WriteException(methodName, frame, exceptionTrace, description, exceptionName);
        }

        public void WriteOperation(string description, string name, string operationMetadata)
        {
            var stackTrace = new StackTrace();

            var stackTraceInfo = StackTraceParser.Parse(
                stackTrace.ToString(),
                (f, t, m, pl, ps, fn, ln) => new
                {
                    Frame = f,
                    Type = t,
                    Method = m,
                    ParameterList = pl,
                    Parameters = ps,
                    File = fn,
                    Line = ln,
                });

            string methodName = null;
            string frame = null;

            bool @break = false;
            foreach (var stackTraceEntry in stackTraceInfo)
            {
                if (@break)
                {
                    methodName = stackTraceEntry.Method;
                    frame = stackTraceEntry.Frame;

                    break;
                }

                var type = stackTraceEntry.Type;
                if (string.IsNullOrEmpty(type)) continue;

                var @typeof = typeof(TraceStepper).Name;
                if (type.EndsWith(@typeof))
                {
                    @break = true;
                }
            }

            _tracer.WriteOperation(methodName, frame, description, name, operationMetadata);
        }

        public void Dispose()
        {
        }
    }
}