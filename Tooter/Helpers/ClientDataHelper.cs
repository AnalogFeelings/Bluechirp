using Mastonet.Entities;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
