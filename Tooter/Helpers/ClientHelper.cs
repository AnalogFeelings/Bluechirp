using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooter.Helpers
{
    public sealed class ClientHelper
    {
        internal static MastodonClient Client { get; private set; }

        internal static void CreateClient(MastodonClient client)
        {
            Client = client;
        }

        internal static void LoadProfile(string clientProfileID)
        {

        }

        internal static bool LoadLastUsedProfile()
        {
            bool isLoadSuccessful = true;

            string lastUsedProfileId = ClientDataHelper.GetLastUsedProfile();
            if (lastUsedProfileId == null)
            {
                isLoadSuccessful = false;
            }
            else
            {
                (AppRegistration appRegistration, Auth auth) = ClientDataHelper.LoadClientProfile(ClientDataHelper.GetLastUsedProfile());
                Client = new MastodonClient(appRegistration, auth);
            }

            return isLoadSuccessful;
        }
    }
}
