using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml;
using TracingExperiment.IOC.Interfaces;
using TracingExperiment.MathService;
using TracingExperiment.Tracing.Interfaces;
using TracingExperiment.Tracing.Utils;

namespace TracingExperiment.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        private readonly IResolver _resolver;
        // GET api/values

        public ValuesController(IResolver resolver)
        {
            _resolver = resolver;
        }

        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            var traceStepper = _resolver.Resolve<ITraceStepper>();
            using (traceStepper)
            {
                traceStepper.WriteMessage(string.Format("Entered controller method with id = {0}", id));
                traceStepper.WriteMessage("Calling wcf service");

                var client = InitializeHttpClient();
                var response = client.GetData(new GetDataRequest
                {
                    value = id
                });

                var result = response.GetDataResult;
                traceStepper.WriteMessage("Finished calling wcf service");
                
                return result;
            }
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }


        /// <summary>
        /// Initialize a basic http binding with 
        /// - transport security mode
        /// - maxed-out request/response buffer sizes 
        /// - Client credential type/proxy credential type - windows 
        /// - Message credential type - username
        /// - Message algo suite - default
        /// </summary>
        private IService1 InitializeHttpClient()
        {
            var serviceBinding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
            {
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport = new HttpTransportSecurity
                    {
                        ClientCredentialType = HttpClientCredentialType.Windows,
                        ProxyCredentialType = HttpProxyCredentialType.Windows,
                        Realm = string.Empty,
                    },
                    Message = new BasicHttpMessageSecurity
                    {
                        AlgorithmSuite = SecurityAlgorithmSuite.Default,
                        ClientCredentialType = BasicHttpMessageCredentialType.UserName
                    },

                },
            };

            var myReaderQuotas1 = new XmlDictionaryReaderQuotas
            {
                MaxStringContentLength = int.MaxValue,
                MaxDepth = int.MaxValue,
                MaxArrayLength = int.MaxValue,
                MaxBytesPerRead = int.MaxValue,
                MaxNameTableCharCount = int.MaxValue
            };
            var url = WebConfigurationManager.AppSettings["client-url"];

            serviceBinding.GetType().GetProperty("ReaderQuotas").SetValue(serviceBinding, myReaderQuotas1, null);

            ChannelFactory<IService1> factory = new ChannelFactory<IService1>(serviceBinding, new EndpointAddress(url));
            factory.Endpoint.Behaviors.Add(_resolver.Resolve<IEndpointBehavior>());

            IService1 channel = factory.CreateChannel();
            return channel;
        }


        private Service1Client InitializeClient()
        {
            var url = WebConfigurationManager.AppSettings["client-url"];
            var serviceBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport)
            {
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue
            };

            var myReaderQuotas1 = new XmlDictionaryReaderQuotas
            {
                MaxStringContentLength = int.MaxValue,
                MaxDepth = int.MaxValue,
                MaxArrayLength = int.MaxValue,
                MaxBytesPerRead = int.MaxValue,
                MaxNameTableCharCount = int.MaxValue
            };

            serviceBinding.GetType().GetProperty("ReaderQuotas").SetValue(serviceBinding, myReaderQuotas1, null);

            var client = new Service1Client(serviceBinding, new EndpointAddress(url));

            var debugBehavior = _resolver.Resolve<IEndpointBehavior>();
            client.Endpoint.EndpointBehaviors.Add(debugBehavior);

            return client;


            //((ClientBase<ResourceSchedulerServiceSoap>)_resourceSchedulerClient).Endpoint.EndpointBehaviors.Add(new TracingMessageBehavior());
        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}
