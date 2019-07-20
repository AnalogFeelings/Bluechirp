using Mastonet.Entities;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Core;
using Windows.Storage;

namespace Tooter.Helpers
{
    public static class ClientDataHelper
    {
        static LocalObjectStorageHelper _localStorageHelper = new LocalObjectStorageHelper();


        const string SavedClientProfilesFileName = "savedClientProfiles.txt";

        // Token settings
        const string AccessTokenString = "accessToken";
        const string CreatedAtString = "createdAt";
        const string ScopeString = "scope";
        const string TokenTypeString = "tokenType";

        // App Registration settings
        const string InstanceString = "instance";
        const string AppScopeString = "appScope";
        const string AppIDString = "appID";

        static async Task StoreClientData(string clientProfileID, Auth token, AppRegistration appRegistration)
        {
            Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
            dataDictionary[AccessTokenString] = token.AccessToken;
            dataDictionary[CreatedAtString] = token.CreatedAt;
            dataDictionary[ScopeString] = token.Scope;
            dataDictionary[TokenTypeString] = token.TokenType;
            dataDictionary[InstanceString] = appRegistration.Instance;
            dataDictionary[AppScopeString] = appRegistration.Scope;
            dataDictionary[AppIDString] = appRegistration.Id;

            _localStorageHelper.Save<object>(clientProfileID, dataDictionary);

            await UpdateSavedClientsFileAsync(clientProfileID);
        }

        private static async Task UpdateSavedClientsFileAsync(string clientProfileID)
        {
            StorageFile fileToSave = await GetSavedClientsFileAsync();
            await FileIO.AppendTextAsync(fileToSave, $"{clientProfileID},");
        }

        private static async Task<StorageFile> GetSavedClientsFileAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var savedClientsFile = await localFolder.CreateFileAsync(SavedClientProfilesFileName, CreationCollisionOption.OpenIfExists);
            return savedClientsFile;
        }

        private static (Auth token, AppRegistration appRegistration) LoadClientProfile(string clientProfileName)
        {
            Auth token = new Auth();
            AppRegistration appRegistration = new AppRegistration();

            // values to load from local storage
            token.AccessToken = _localStorageHelper.Read<string>(clientProfileName, AccessTokenString);
            token.CreatedAt = _localStorageHelper.Read<string>(clientProfileName, CreatedAtString);
            token.Scope = _localStorageHelper.Read<string>(clientProfileName, ScopeString);
            token.TokenType = _localStorageHelper.Read<string>(clientProfileName, TokenTypeString);

            appRegistration.Id = _localStorageHelper.Read<long>(clientProfileName, AppIDString);
            appRegistration.Instance = _localStorageHelper.Read<string>(clientProfileName, InstanceString);
            appRegistration.Scope = _localStorageHelper.Read<Mastonet.Scope>(clientProfileName, ScopeString);


            // Values to load from constants.
            appRegistration.ClientId = APIKeys.ClientID;
            appRegistration.ClientSecret = APIKeys.ClientSecret;
            appRegistration.RedirectUri = APIKeys.RedirectUri;

            return (token, appRegistration);
        }

        private static async Task<string[]> GetAvailableClientProfiles()
        {
            StorageFile clientProfilesFile = await GetSavedClientsFileAsync();
            string fileContents = await FileIO.ReadTextAsync(clientProfilesFile);
            string[] clientProfileList = fileContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return clientProfileList;
        }
    }
}
