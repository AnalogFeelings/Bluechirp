using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BluechirpLib.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BluechirpLib.Helpers
{
    public sealed class ClientHelper
    {
        public static string LoadedProfile { get; private set; } = "";

        public static MastodonClient Client { get; private set; }

        public static void CreateClient(MastodonClient client)
        {
            Client = client;
        }

        public static void LoadProfile(string clientProfileID)
        {
            (AppRegistration appRegistration, Auth auth) = ClientDataHelper.LoadClientProfile(clientProfileID);
            Client = new MastodonClient(appRegistration.Instance, auth.AccessToken);
            LoadedProfile = clientProfileID;
        }

        public static bool LoadLastUsedProfile()
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

        public static async Task MakeLogoutPreprationsAsync()
        {
            await ClientDataHelper.RemoveClientProfileAsync(LoadedProfile);
            await ClientDataHelper.ClearTimelineCache();
        }

        public static void SetLoadedProfile(string clientProfileID)
        {
            LoadedProfile = clientProfileID;
        }
    }
}

