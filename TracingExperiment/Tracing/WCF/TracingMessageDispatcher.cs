using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Threading.Tasks;
using TracingExperiment.Tracing.Interfaces;

namespace TracingExperiment.Tracing.WCF
{
    /// <summary>
    /// DebugMessageDispatcher class - implements IDispatchMessageInspector interface
    /// </summary>
    public class TracingMessageDispatcher : IDispatchMessageInspector, IMessageLogging
    {
        private readonly IHelper _helper;
        private readonly ITraceStepper _logger;

        /// <summary>
        /// DebugMessageDispatcher constructor
        /// </summary>
        public TracingMessageDispatcher(IHelper helper, ITraceStepper logger)
        {
            _helper = helper;
            _logger = logger;
        }

        /// <summary>
        /// AfterReceiveRequest method
        /// When a service call is being traced and logged, this is called after the client has sent the request but before the request is processed by the service
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <param name="instanceContext"></param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            try
            {
                var requestBuffer = request.CreateBufferedCopy(Int32.MaxValue);
                var requestCopyForLogging = requestBuffer.CreateMessage();
                request = requestBuffer.CreateMessage();

                // Since this is .NET 4.0, cannot use Task.Run
                // Using Task.Factory.StartNew instead
                Task.Factory.StartNew(() => StartLoggingTheRequest(requestCopyForLogging));
            }
            catch (Exception ex)
            {
                _logger.WriteException(ex, "Failure logging the request (AfterReceiveRequest)");
                throw;
            }

            return request;
        }

        /// <summary>
        /// StartLoggingTheRequest method
        /// Used to start the process of logging with a copy of the request Message object 
        /// Exposed a public method to allow for consumption by other behavior extensions
        /// </summary>
        /// <param name="requestCopyForLogging"></param>
        public void StartLoggingTheRequest(Message requestCopyForLogging)
        {
            try
            {
                if (_helper.ShouldLog)
                {
                    _logger.WriteOperation("Outgoing wcf request ", Keys.WcfClientNameKey, MessageUtils.MessageToXml(requestCopyForLogging));
                }
            }
            catch (Exception ex)
            {
                _logger.WriteException(ex, "Failure logging the request (StartLoggingTheRequest)");
                throw;
            }
        }
        
        /// <summary>
        /// BeforeSendReply method
        /// When a service call is being traced and logged, this is called after the service has processed the request but before the response is sent back to the client
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            try
            {
                var replyBuffer = reply.CreateBufferedCopy(Int32.MaxValue);
                var replyCopyForLogging = replyBuffer.CreateMessage();
                reply = replyBuffer.CreateMessage();

                // Since this is .NET 4.0, cannot use Task.Run
                // Using Task.Factory.StartNew instead
                Task.Factory.StartNew(() => StartLoggingTheReply(replyCopyForLogging));
            }
            catch (Exception ex)
            {
                _logger.WriteException(ex, "Failure logging the reply (BeforeSendReply)");
                throw;
            }
        }

        /// <summary>
        /// StartLoggingTheReply method
        /// Used to start the process of logging with a copy of the reply Message object 
        /// Exposed a public method to allow for consumption by other behavior extensions
        /// </summary>
        /// <param name="replyCopyForLogging"></param>
        public void StartLoggingTheReply(Message replyCopyForLogging)
        {
            try
            {
                if (_helper.ShouldLog)
                {
                    _logger.WriteOperation("Incoming wcf reply ", Keys.WcfClientNameKey, MessageUtils.MessageToXml(replyCopyForLogging));
                }
            }
            catch (Exception ex)
            {
                _logger.WriteException(ex, "Failure logging the reply (StartLoggingTheReply)");
                throw;
            }
        }
    }
}
