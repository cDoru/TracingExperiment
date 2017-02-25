using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TracingExperiment.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string MyActionLink(this HtmlHelper helper, string actionName, string controllerName, string linkText)
        {
            var qs = helper.ViewContext.HttpContext.Request.QueryString;
            RouteValueDictionary dict = new RouteValueDictionary();
            qs.AllKeys.ToList().ForEach(k => dict.Add(k, qs[k]));

            var urlhelper = new UrlHelper(helper.ViewContext.RequestContext);

            string url = urlhelper.Action(actionName, controllerName, dict);
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = (!String.IsNullOrEmpty(linkText)) ? HttpUtility.HtmlEncode(linkText) : String.Empty
            };
            tagBuilder.MergeAttribute("href", url);
            return tagBuilder.ToString(TagRenderMode.Normal);

        }
    }
}