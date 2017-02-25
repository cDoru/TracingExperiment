namespace TracingExperiment.Tracing.Interfaces
{
    /// <summary>
    /// IHelper interface
    /// General utility methods to reduce code duplication
    /// </summary>
    public interface IHelper
    {
        /// <summary>
        /// ShouldLog method stub - reads the config to determine if we should log or not
        /// </summary>
        /// <returns></returns>
        bool ShouldLog();
    }
}
