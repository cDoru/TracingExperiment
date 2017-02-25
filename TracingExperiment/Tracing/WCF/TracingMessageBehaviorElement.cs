using System;
using System.ServiceModel.Configuration;
using System.ServiceModel.Dispatcher;

namespace TracingExperiment.Tracing.WCF
{
    /// <summary>
    /// DebugMessageBehaviorElement class - extends BehaviorExtensionElement abstract class
    /// </summary>
    public class TracingMessageBehaviorElement : BehaviorExtensionElement
    {
        private readonly IClientMessageInspector _clientMessageInspector;
        private readonly IDispatchMessageInspector _debugMessageDispatcher;

        /// <summary>
        /// TracingMessageBehavior constructor
        /// </summary>
        public TracingMessageBehaviorElement(
            IClientMessageInspector clientMessageInspector, 
            IDispatchMessageInspector debugMessageDispatcher)
        {
            _clientMessageInspector = clientMessageInspector;
            _debugMessageDispatcher = debugMessageDispatcher;
        }

        /// <summary>
        /// CreateBehavior method, overrides base method
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new TracingMessageBehavior(_clientMessageInspector, _debugMessageDispatcher);
        }

        /// <summary>
        /// BehaviorType public property, overrides base property
        /// </summary>
        /// <returns></returns>
        public override Type BehaviorType
        {
            get
            {
                return typeof(TracingMessageBehavior);
            }
        }
    }

}
