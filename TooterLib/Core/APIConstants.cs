using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooter.Core
{
    public class APIConstants
    {

        internal static string AppName = "null";
        internal static string AppWebsite = "null";
        internal static string RedirectUri = "null";


        public static void SetConstants(string appName, string appWebsite, string redirectUri)
        {
            AppName = appName;
            AppWebsite = appWebsite;
            RedirectUri = redirectUri;
        }


    }
}
