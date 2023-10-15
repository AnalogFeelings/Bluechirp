using System.Collections.Generic;

namespace Bluechirp.Library.Constants
{
    /// <summary>
    /// Contains setting keys and default values.
    /// </summary>
    public static class SettingsConstants
    {
        /// <summary>
        /// Used to remember the last user profile.
        /// </summary>
        public const string LAST_PROFILE_KEY = "LastProfile";

        /// <summary>
        /// Read-only dictionary containing all the default
        /// values for all setting keys.
        /// </summary>
        public static readonly IReadOnlyDictionary<string, object> Defaults = new Dictionary<string, object>()
        {
            { LAST_PROFILE_KEY, string.Empty }
        };
    }
}
