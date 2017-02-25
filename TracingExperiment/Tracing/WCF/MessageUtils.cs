using System.IO;
using System.ServiceModel.Channels;
using System.Xml;

namespace TracingExperiment.Tracing.WCF
{
    public static class MessageUtils
    {
        public static string MessageToXml(Message message)
        {
            // read the message into an XmlDocument as then you can work with the contents 
            return ToStringRepresentation(ReadMessageIntoXmlDocument(message));
        }

        private static string ToStringRepresentation(XmlNode document)
        {
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                document.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        private static XmlDocument ReadMessageIntoXmlDocument(Message message)
        {
            // read the message into an XmlDocument as then you can work with the contents 
            // Message is a forward reading class only so once read that's it. 
            var myMemoryStream = new MemoryStream();
            var myXmlWriter = XmlWriter.Create(myMemoryStream);
            message.WriteMessage(myXmlWriter);
            myXmlWriter.Flush();
            myMemoryStream.Position = 0;
            var myXmlDocument = new XmlDocument { PreserveWhitespace = true };
            myXmlDocument.Load(myMemoryStream);

            return myXmlDocument;
        }

    }
}