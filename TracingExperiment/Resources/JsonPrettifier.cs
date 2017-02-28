using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TracingExperiment.Resources
{
    public class JsonPrettifier
    {
        private const string ReplateToken = "{JSON_HERE}";
        private const string Filename = "json.html";

        static public string BeautifyJson(string json)
        {
            string jsonFormatted = JToken.Parse(json).ToString(Formatting.Indented);
            var formatFile = GetEmbeddedJsonTemplate(Filename);
            return formatFile.Replace(ReplateToken, jsonFormatted);
        }


        private static string GetEmbeddedJsonTemplate(string resource)
        {
            using (var stream = EmbeddedResources.ThisResources.GetStream(resource))
            {
                using (var sReader = new StreamReader(stream))
                {
                    var content = sReader.ReadToEnd();
                    return content;
                }
            }
        }
    }
}