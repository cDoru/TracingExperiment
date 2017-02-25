using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using Newtonsoft.Json;
using TracingExperiment.Tracing.Interfaces;

namespace TracingExperiment.Tracing.Handlers
{
    public class ApiLogHandler : DelegatingHandler
    {
        private readonly ITracer _tracer;
        private readonly ITraceStepper _traceStepper;

        public ApiLogHandler(ITracer tracer, ITraceStepper traceStepper)
        {
            _tracer = tracer;
            _traceStepper = traceStepper;
        }

        private void ProcessRequest(Task<string> task, ApiLogEntry apiLogEntry )
        {
            apiLogEntry.RequestContentBody = task.Result;
            _traceStepper.WriteOperation("Web API request", "request headers", apiLogEntry.RequestHeaders);
            _traceStepper.WriteOperation("Web API request", "query string", apiLogEntry.RequestUri);
            _traceStepper.WriteOperation("Web API request", "body request", apiLogEntry.RequestContentBody);
        }

        private HttpResponseMessage ProcessResponse(Task<HttpResponseMessage> task, ApiLogEntry apiLogEntry)
        {
            var response = task.Result;

            // Update the API log entry with response info
            apiLogEntry.ResponseStatusCode = (int)response.StatusCode;
            apiLogEntry.ResponseTimestamp = DateTime.Now;

            if (response.Content != null)
            {
                apiLogEntry.ResponseContentBody = response.Content.ReadAsStringAsync().Result;
                apiLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                apiLogEntry.ResponseHeaders = SerializeHeaders(response.Content.Headers);
            }

            // TODO: Save the API log entry to the database

            _traceStepper.WriteOperation("Web API response", "response body", apiLogEntry.ResponseContentBody);
            _traceStepper.WriteOperation("Web API response", "response headers", apiLogEntry.ResponseHeaders);

            _traceStepper.Dispose();

            var represetnation = _tracer.ToStringRepresentation();

            return response;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var apiLogEntry = CreateApiLogEntryWithRequestData(request);

            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task =>
                    {
                        ProcessRequest(task, apiLogEntry);
                    }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken).ContinueWith(task => ProcessResponse(task, apiLogEntry), cancellationToken);
        }

        private ApiLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request)
        {
            var context = ((HttpContextBase) request.Properties["MS_HttpContext"]);
            var routeData = request.GetRouteData();

            return new ApiLogEntry
            {
                Application = "Web API",
                User = context.User.Identity.Name,
                Machine = Environment.MachineName,
                RequestContentType = context.Request.ContentType,
                RequestRouteTemplate = routeData.Route.RouteTemplate,
                RequestRouteData = SerializeRouteData(routeData),
                RequestIpAddress = context.Request.UserHostAddress,
                RequestMethod = request.Method.Method,
                RequestHeaders = SerializeHeaders(request.Headers),
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };
        }

        private string SerializeRouteData(IHttpRouteData routeData)
        {
            return JsonConvert.SerializeObject(routeData, Formatting.Indented);
        }

        private static string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value == null) continue;
                
                var header = item.Value.Aggregate(String.Empty, (current, value) => current + (value + " "));
                // Trim the trailing space and add item to the dictionary
                header = header.TrimEnd(" ".ToCharArray());
                dict.Add(item.Key, header);
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }
    }
}