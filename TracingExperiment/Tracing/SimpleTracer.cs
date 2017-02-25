using System;
using System.Xml.Linq;
using TracingExperiment.Tracing.Interfaces;
using TracingExperiment.Tracing.Utils;

namespace TracingExperiment.Tracing
{
    public class SimpleTracer : ITracer
    {
        private readonly object _thisLock = new Object();
        private readonly XElement _root;
        private readonly XDocument _document;

        public SimpleTracer()
        {
            using (new TimingOutLock(_thisLock))
            {
                _document = new XDocument();
                _root = new XElement("workflow");
                _document.Add(_root);
            }
        }

        public void WriteMessage(string source, string frame, string message)
        {
            var messageElement = new XElement("message");


            if (!string.IsNullOrEmpty(source))
            {
                messageElement.Add(new XAttribute("source", source));
            }

            if (!string.IsNullOrEmpty(frame))
            {
                messageElement.Add(new XAttribute("frame", frame));
            }

            messageElement.Add(new XElement("value", message));
            _root.Add(messageElement);
        }

        public void WriteException(string source, string frame, string exception, string description, string name)
        {
            var messageElement = new XElement("exception");

            if (!string.IsNullOrEmpty(source))
            {
                messageElement.Add(new XAttribute("source", source));
            }

            if (!string.IsNullOrEmpty(frame))
            {
                messageElement.Add(new XAttribute("frame", frame));
            }

            if (!string.IsNullOrEmpty(description))
            {
                messageElement.Add(new XAttribute("description", description));
            }

            if (!string.IsNullOrEmpty("name"))
            {
                messageElement.Add(new XAttribute("name", name));
            }

            messageElement.Add(new XElement("value", exception));
            _root.Add(messageElement);
        }

        public void WriteOperation(string source, string frame, string description, string name, string operationMetadata)
        {
            var messageElement = new XElement("operation");

            if (!string.IsNullOrEmpty(source))
            {
                messageElement.Add(new XAttribute("source", source));
            }

            if (!string.IsNullOrEmpty(frame))
            {
                messageElement.Add(new XAttribute("frame", frame));
            }

            if (!string.IsNullOrEmpty(description))
            {
                messageElement.Add(new XAttribute("description", description));
            }

            if (!string.IsNullOrEmpty("name"))
            {
                messageElement.Add(new XAttribute("name", name));
            }

            messageElement.Add(new XElement("value", operationMetadata));
            _root.Add(messageElement);
        }


        public string ToStringRepresentation()
        {
            var reader = _document.CreateReader();
            reader.MoveToContent();
            return reader.ReadInnerXml();
        }
    }
}