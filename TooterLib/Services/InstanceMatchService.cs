using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tooter.Services
{
    public static class InstanceMatchService
    {
        const string InstanceNameRegularExpression = "^[A-Za-z0-9\\-]+\\.+[A-Za-z0-9\\-]+$";

        public static bool CheckIfInstanceNameIsProperlyFormatted(string instanceName)
        {
            bool wasFormattedProperly = false;

            // Define a regular expression for repeated words.
            Regex rx = new Regex(InstanceNameRegularExpression,
              RegexOptions.Compiled | RegexOptions.IgnoreCase);


            // Find matches.
            MatchCollection matches = rx.Matches(instanceName);

            if(matches.Count > 0)
            {
                wasFormattedProperly = true;
            }

            return wasFormattedProperly;
        }
    }
}
