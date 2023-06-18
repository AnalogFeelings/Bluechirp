using System.Text.RegularExpressions;

namespace Bluechirp.Library.Services
{
    public class InstanceMatchService
    {
        private const string INSTANCE_REGEX_STRING = "^[A-Za-z0-9\\-]+\\.+[A-Za-z0-9\\-]+$";
        private readonly Regex _instanceRegex = new Regex(INSTANCE_REGEX_STRING, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool CheckIfInstanceNameIsProperlyFormatted(string instanceName)
        {
            bool wasFormattedProperly = false;

            // Find matches.
            MatchCollection matches = _instanceRegex.Matches(instanceName);

            if (matches.Count > 0)
            {
                wasFormattedProperly = true;
            }

            return wasFormattedProperly;
        }
    }
}
