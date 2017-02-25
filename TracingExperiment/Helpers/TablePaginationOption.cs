namespace TracingExperiment.Helpers
{
    ///<summary>
    ///Represents the options available for client or server side pagination.
    ///</summary>
    public enum TablePaginationOption
    {
        /// <summary></summary>
        [ValueField(Name = "data-side-pagination", Value = "")]
        none,
        /// <summary></summary>
        [ValueField(Name = "data-side-pagination", Value = "client")]
        client,
        /// <summary></summary>
        [ValueField(Name = "data-side-pagination", Value = "server")]
        server,
    }
}