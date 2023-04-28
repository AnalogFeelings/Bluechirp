using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluechirp.Core
{
    public class APIConstants
    {
        public const string RedirectUri = "aestheticalz-bluechirp://mycallback";
        public const string AppName = "Bluechirp";
        public const string AppWebsite = "https://github.com/AestheticalZ/Bluechirp";

        internal static void SetAPIConstants()
        {
            Library.Core.APIConstants.SetConstants(AppName, AppWebsite, RedirectUri);
        }
    }
}
