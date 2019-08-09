using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Services;
using Tooter.View;

namespace Tooter.Helpers
{
    public sealed class ClientHelper
    {
        static string loadedProfile = "";
        internal static MastodonClient Client { get; private set; }

        internal static void CreateClient(MastodonClient client)
        {
            Client = client;
        }

        internal static void LoadProfile(string clientProfileID)
        {
            (AppRegistration appRegistration, Auth auth) = ClientDataHelper.LoadClientProfile(clientProfileID);
            Client = new MastodonClient(appRegistration, auth);
            loadedProfile = clientProfileID;
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
                LoadProfile(ClientDataHelper.GetLastUsedProfile());
                
            }

            return isLoadSuccessful;
        }

        internal static async Task Logout()
        {
            await ClientDataHelper.RemoveClientProfileAsync(loadedProfile);
            NavService.Instance.Navigate(typeof(LoginView));
        }
    }
}
