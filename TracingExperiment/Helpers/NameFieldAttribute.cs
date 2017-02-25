using System;

namespace TracingExperiment.Helpers
{
    /// <exclude/>
    [AttributeUsage(AttributeTargets.Field)]
    internal class NameFieldAttribute : Attribute
    {
        /// <exclude/>
        public string Name { get; set; }
    }
}