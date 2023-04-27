using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastonet;
namespace Bluechirp.Helpers
{
    public static class TootVisibilityHelper
    {
        public static Dictionary<Mastonet.Visibility, string> TootVisibilityMap = new Dictionary<Visibility, string>()
        {
            {Visibility.Public,"\xE909" },
            {Visibility.Unlisted, "\xE785"},
            {Visibility.Private,"\xE72E" },
            {Visibility.Direct, "\xE724"},
        };
    }
}
