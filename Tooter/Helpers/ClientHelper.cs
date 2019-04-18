using Mastonet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooter.Helpers
{
    public sealed class ClientHelper
    {
        internal static MastodonClient _client { get; private set; }
        
        internal static void CreateClient(MastodonClient client)
        {
            _client = client;
        }
    }
}
