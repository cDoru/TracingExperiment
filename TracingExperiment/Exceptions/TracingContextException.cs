using System;
using System.Runtime.Serialization;

namespace TracingExperiment.Exceptions
{
    [Serializable]
    public class TracingContextException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TracingContextException()
        {
        }

        public TracingContextException(string message) : base(message)
        {
        }

        public TracingContextException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TracingContextException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}