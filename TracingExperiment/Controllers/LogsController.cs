using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using TracingExperiment.Models;
using TracingExperiment.Tracing.Database;
using TracingExperiment.Tracing.Database.Interfaces;
using TracingExperiment.Tracing.Utils.Interfaces;

namespace TracingExperiment.Controllers
{
    public class LogsController : Controller
    {
        private const int LogLimit = 1*24; // 1 day
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
            const string ascending = "asc";
            const string descending = "desc";
            // get logs newer than 7 days

            var nowUtc = _now.UtcNow;

            var traces =
                _context.AsQueryable<LogEntry>()
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
                                                         || x.TraceData.Contains(search))
                .Skip((offset / limit) * limit)
                .Take(limit).ToList();
            }

            List<TraceViewModel> tracesVms = new List<TraceViewModel>();

            foreach (var trace in pagedTracesList)
            {
                var item = new TraceViewModel
                {
                    Duration =
                        string.Format("{0} seconds",
                            (trace.ResponseTimestamp.Value - trace.RequestTimestamp.Value).TotalSeconds.ToString("#.##")),
                    Timestamp = trace.Timestamp.ToString(CultureInfo.InvariantCulture),
                    Uri = trace.RequestUri,
                    Workflow = "no workflow yet"
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
    }
}