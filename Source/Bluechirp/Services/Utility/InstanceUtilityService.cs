using Bluechirp.Library.Services.Utility;
using System.Text.RegularExpressions;

namespace Bluechirp.Services.Utility
{
    /// <summary>
    /// Implements a utility server for Mastodon instance
    /// names.
    /// </summary>
    internal class InstanceUtilityService : IInstanceUtilityService
    {
        private const string INSTANCE_REGEX_STRING = "^[A-Za-z0-9\\-]+\\.+[A-Za-z0-9\\-]+$";
        private readonly Regex _instanceRegex = new Regex(INSTANCE_REGEX_STRING, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <inheritdoc/>
        public bool CheckInstanceName(string instanceName)
        {
            MatchCollection matches = _instanceRegex.Matches(instanceName);

            return matches.Count > 0;
        }
    }
}
