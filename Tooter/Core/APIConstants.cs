using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooter.Core
{
    public class APIConstants
    {
        public const string RedirectUri = "tooter://mycallback";
        public const string AppName = "Fedi (Beta)";
        public const string AppWebsite = "https://www.microsoft.com/store/apps/9ND02K1RC5CZ";



        // Switch to this when you release the app
        //public const string AppName = "Fedi";
        //public const string AppWebsite = "https://www.microsoft.com/store/apps/9P3T3N4JBK91";

        internal static void SetAPIConstants()
        {
            TooterLib.Core.APIConstants.SetConstants(AppName, AppWebsite, RedirectUri);
        }

    }
}
