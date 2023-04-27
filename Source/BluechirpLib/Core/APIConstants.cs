using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluechirpLib.Core
{
    public class APIConstants
    {

        public static string AppName = "null";
        public static string AppWebsite = "null";
        public static string RedirectUri = "null";


        public static void SetConstants(string appName, string appWebsite, string redirectUri)
        {
            AppName = appName;
            AppWebsite = appWebsite;
            RedirectUri = redirectUri;
        }


    }
}
