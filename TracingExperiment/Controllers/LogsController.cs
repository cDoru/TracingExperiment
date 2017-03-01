using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TracingExperiment.Models;
using TracingExperiment.Resources;
using TracingExperiment.Tracing.Database;
using TracingExperiment.Tracing.Database.Interfaces;
using TracingExperiment.Tracing.Utils.Interfaces;

namespace TracingExperiment.Controllers
{
    public class LogsController : Controller
    {
        private const int LogLimit = 2*24; // 2 days
        private readonly ITracingContext _context;
        private readonly INow _now;

        public LogsController(ITracingContext context, INow now)
        {
            _context = context;
            _now = now;
        }

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetTracesPaged(int offset, int limit, string search, string sort, string order)
        {
            const string xmlHeader8 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            const string xmlHeader16 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            const string ascending = "asc";
            const string descending = "desc";
            // get logs newer than 7 days

            var nowUtc = _now.UtcNow;

            var traces =
                _context.AsQueryable<LogEntry>().Include(x => x.Steps)
                    .Where(x => DbFunctions.AddHours(x.Timestamp, LogLimit) > nowUtc)
                    .AsQueryable();

            IOrderedQueryable<LogEntry> pagedTraces;
            if (order == ascending)
            {
                pagedTraces = traces.OrderBy(x => x.Timestamp);
            }
            else if (order == descending)
            {
                pagedTraces = traces.OrderByDescending(x => x.Timestamp);
            }
            else
            {
                pagedTraces = traces.OrderByDescending(x => x.Timestamp);
            }

            List<LogEntry> pagedTracesList;

            if (string.IsNullOrEmpty(search))
            {
                pagedTracesList = pagedTraces
                .Skip((offset / limit) * limit)
                .Take(limit).ToList();
            }
            else
            {
                pagedTracesList = pagedTraces.Where(x => x.RequestUri.Contains(search)
                                                         || x.Steps.Any(y => y.Metadata != null && y.Metadata.Contains(search)))
                .Skip((offset / limit) * limit)
                .Take(limit).ToList();
            }

            List<TraceViewModel> tracesVms = new List<TraceViewModel>();

            foreach (var trace in pagedTracesList)
            {
                var traceSteps = trace.Steps.OrderBy(x => x.Index).ToList();

                StringBuilder builder = new StringBuilder();
                builder.Append("<p style=\"white-space: nowrap;\">Start request </p>");

                foreach (var tracestep in traceSteps)
                {
                    builder.Append(string.Format("<p style=\"white-space: nowrap;\">{0}</p>", string.Format("From {0} method located in frame {1} {2} {3} \r\n", tracestep.Source,
                        tracestep.Frame,
                        (!string.IsNullOrEmpty(tracestep.Name) ? string.Format(" (which processes {0}) ", tracestep.Name) : ""),
                        (!string.IsNullOrEmpty(tracestep.Message) ? string.Format(" (with message {0}) ", tracestep.Message) : ""))));

                    if (!string.IsNullOrEmpty(tracestep.Metadata))
                    {
                        builder.Append("<p style=\"white-space: nowrap;\">With metadata: </p>");

                        string beautified;

                        if (IsValidXml(tracestep.Metadata))
                        {
                            // xml 
                            // operation metadata is xml in our case
                             beautified = XmlPrettifyHelper.Prettify(tracestep.Metadata.Replace(xmlHeader8, "").Replace(xmlHeader16, ""));
                        }
                        else if (IsValidJson(tracestep.Metadata))
                        {
                            beautified = JsonPrettifier.BeautifyJson(tracestep.Metadata);
                        }
                        else beautified = tracestep.Metadata;

                        builder.Append(beautified);
                    }

                }

                builder.Append("<p style=\"white-space: nowrap;\">End request </p>");

                var traceString = HttpUtility.HtmlEncode(builder.ToString());

                var item = new TraceViewModel
                {
                    Duration =
                        string.Format("{0} seconds",
                            (trace.ResponseTimestamp.Value - trace.RequestTimestamp.Value).TotalSeconds.ToString("#.##")),
                    Timestamp = trace.Timestamp.ToString(CultureInfo.InvariantCulture),
                    Uri = trace.RequestUri,
                    Workflow = new HtmlString(HttpUtility.HtmlDecode(traceString)).ToHtmlString()
                };

                tracesVms.Add(item);
            }

            var model = new
            {
                total = traces.Count(),
                rows = tracesVms
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private static bool IsValidXml(string xmlString)
        {
            var tagsWithData = new Regex("<\\w+>[^<]+</\\w+>");

            //Light checking
            if (string.IsNullOrEmpty(xmlString) || tagsWithData.IsMatch(xmlString) == false)
            {
                return false;
            }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                return true;
            }
            catch (Exception e1)
            {
                return false;
            }
        }

        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    return false;
                }
            }
            return false;
        }
    }
}