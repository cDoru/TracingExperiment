using System;

namespace TracingExperiment.Helpers
{
    /// <exclude/>
    [AttributeUsage(AttributeTargets.Field)]
    internal class ValueFieldAttribute : NameFieldAttribute
    {
        /// <exclude/>
        public string Value { get; set; }
    }
}