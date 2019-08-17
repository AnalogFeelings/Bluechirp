using Mastonet.Entities;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooterLib.Core;
using TooterLib.Enums;
using TooterLib.Model;
using Windows.Storage;

namespace TooterLib.Helpers
{
    public static class ClientDataHelper
    {
        static ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        static LocalObjectStorageHelper _localStorageHelper = new LocalObjectStorageHelper();
        static TempObjectStorageHelper _tempStorageHelper = new TempObjectStorageHelper();

        public static HashSet<string> ClientProfileList { get; } = new HashSet<string>();
        public static string LastUsedProfile { get; private set; } = null;

        const string SavedClientProfilesFileName = "savedClientProfiles.txt";

        public async static Task StoreTimelineCache(TimelineCache cachedTimeline)
        {
            var timelineType = cachedTimeline.CurrentTimelineSettings.CurrentTimelineType;
            await _tempStorageHelper.SaveFileAsync(timelineType.ToString(), cachedTimeline);
        }

        public static async Task<(bool wasTimelineLoaded, TimelineCache cacheToReturn)> LoadTimelineFromFileAsync(TimelineType timelineType)
        {
            bool wasTimelineLoaded = false;
            TimelineCache cacheToReturn = null;


            try
            {
                cacheToReturn = await _tempStorageHelper.ReadFileAsync<TimelineCache>(timelineType.ToString());
                if (cacheToReturn != null)
                {
                    wasTimelineLoaded = true;
                }
            }
            catch (Exception)
            {


            }

            return (wasTimelineLoaded, cacheToReturn);
        }

        public static async Task ClearTimelineCache()
        {
            var cacheData = await ApplicationData.Current.TemporaryFolder.GetItemsAsync();
            foreach (var item in cacheData)
            {
                await item.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
        }

        // Token settings
        const string AccessTokenString = "accessToken";
        const string CreatedAtString = "createdAt";
        const string ScopeString = "scope";
        const string TokenTypeString = "tokenType";

        // App Registration settings
        const string InstanceString = "instance";
        const string AppScopeString = "appScope";
        const string AppIDString = "appID";
        const string ClientIDString = "clientID";
        const string ClientSecretString = "clientSecret";

        // Profile swapping settings
        const string LastUsedProfileString = "lastUsedProfile";

        public async static Task StartUpAsync()
        {
            await LoadClientProfiles();
        }

        public static void SetLastUsedProfile(string clientProfileID)
        {
            _localStorageHelper.Save(LastUsedProfileString, clientProfileID);
        }

        public static string GetLastUsedProfile()
        {
            LastUsedProfile = _localStorageHelper.Read<string>(LastUsedProfileString);
            return LastUsedProfile;
        }

        public static async Task StoreClientData(string clientProfileID, Auth token, AppRegistration appRegistration)
        {
            if (!ClientProfileList.Contains(clientProfileID))
            {

                int appScopeInt = (int)appRegistration.Scope;

                Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
                dataDictionary[AccessTokenString] = token.AccessToken;
                dataDictionary[CreatedAtString] = token.CreatedAt;
                dataDictionary[ScopeString] = token.Scope;
                dataDictionary[TokenTypeString] = token.TokenType;
                dataDictionary[InstanceString] = appRegistration.Instance;
                dataDictionary[AppScopeString] = appScopeInt;
                dataDictionary[AppIDString] = appRegistration.Id;
                dataDictionary[ClientIDString] = appRegistration.ClientId;
                dataDictionary[ClientSecretString] = appRegistration.ClientSecret;

                _localStorageHelper.Save<object>(clientProfileID, dataDictionary);


                await AddClientProfileAsync(clientProfileID);
            }

        }

        private static void RemoveClientData(string clientProfileID)
        {
            _localSettings.DeleteContainer(clientProfileID);
        }

        public static async Task AddClientProfileAsync(string clientProfileID)
        {
            ClientProfileList.Add(clientProfileID);
            StorageFile fileToSave = await GetSavedClientsFileAsync();
            await FileIO.AppendTextAsync(fileToSave, $"{clientProfileID},");
        }

        public static async Task RemoveClientProfileAsync(string clientProfileID)
        {
            ClientProfileList.Remove(clientProfileID);

            StringBuilder contentBuilder = new StringBuilder();
            for (int i = 0; i < ClientProfileList.Count; i++)
            {
                contentBuilder.Append($"{ClientProfileList.ElementAt(i)},");
            }

            StorageFile profilesFile = await GetSavedClientsFileAsync();
            if (GetLastUsedProfile() == clientProfileID)
            {
                TryClearLastUsedProfile();
            }
            RemoveClientData(clientProfileID);
            await FileIO.WriteTextAsync(profilesFile, contentBuilder.ToString());
        }

        private static void TryClearLastUsedProfile()
        {
            if (ClientProfileList.Count > 0)
            {
                SetLastUsedProfile(ClientProfileList.First());
            }
            else
            {
                SetLastUsedProfile(null);
            }
        }

        private static async Task<StorageFile> GetSavedClientsFileAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var savedClientsFile = await localFolder.CreateFileAsync(SavedClientProfilesFileName, CreationCollisionOption.OpenIfExists);
            return savedClientsFile;
        }

        public static (AppRegistration appRegistration, Auth token) LoadClientProfile(string clientProfileID)
        {
            Auth token = new Auth();
            AppRegistration appRegistration = new AppRegistration();

            // values to load from local storage
            token.AccessToken = _localStorageHelper.Read<string>(clientProfileID, AccessTokenString, default(string));
            token.CreatedAt = _localStorageHelper.Read(clientProfileID, CreatedAtString, default(string));
            token.Scope = _localStorageHelper.Read(clientProfileID, ScopeString, default(string));
            token.TokenType = _localStorageHelper.Read(clientProfileID, TokenTypeString, default(string));

            appRegistration.Id = _localStorageHelper.Read<long>(clientProfileID, AppIDString, default(long));
            appRegistration.Instance = _localStorageHelper.Read(clientProfileID, InstanceString, default(string));

            appRegistration.ClientId = _localStorageHelper.Read(clientProfileID, ClientIDString, default(string));
            appRegistration.ClientSecret = _localStorageHelper.Read(clientProfileID, ClientSecretString, default(string));


            int appScopeInt = _localStorageHelper.Read<int>(clientProfileID, AppScopeString, default(int));
            appRegistration.Scope = (Mastonet.Scope)appScopeInt;

            // Values to load from constants.
            appRegistration.RedirectUri = APIConstants.RedirectUri;


            SetLastUsedProfile(clientProfileID);
            return (appRegistration, token);
        }

        private async static Task LoadClientProfiles()
        {
            StorageFile clientProfilesFile = await GetSavedClientsFileAsync();
            string fileContents = await FileIO.ReadTextAsync(clientProfilesFile);
            string[] loadedProfiles = fileContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var profile in loadedProfiles)
            {
                ClientProfileList.Add(profile);
            }

        }


    }
}

