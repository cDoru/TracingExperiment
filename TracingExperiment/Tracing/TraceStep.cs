namespace TracingExperiment.Tracing
{
    internal class TraceStep
    {
        public string Description { get; set; }

        public string OperationName { get; set; }

        public string OperationMetadata { get; set; }

        public string Exception { get; set; }
    }
}