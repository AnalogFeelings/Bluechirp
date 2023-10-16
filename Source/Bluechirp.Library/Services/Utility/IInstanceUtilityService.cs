namespace Bluechirp.Library.Services.Utility
{
    /// <summary>
    /// Defines a service interface for utility
    /// methods regarding Mastodon instances.
    /// </summary>
    public interface IInstanceUtilityService
    {
        /// <summary>
        /// Checks if an instance name is formatted
        /// correctly.
        /// </summary>
        /// <param name="instanceName">The instance name.</param>
        /// <returns>
        /// <see langword="true"/> if the name is properly formatted.
        /// </returns>
        public bool CheckInstanceName(string instanceName);
    }
}
