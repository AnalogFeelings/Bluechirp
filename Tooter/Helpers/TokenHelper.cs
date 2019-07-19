using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Tooter.Helpers
{
    public static class TokenHelper
    {

       static ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        const string AccessTokenString = "accessToken";
        const string CreatedAtString = "createdAt";
        const string ScopeString = "scope";
        const string TokenTypeString = "tokenType";

        public static void SaveToken(Auth tokenToSave)
        {
            SaveToLocalSettings<string>(AccessTokenString, tokenToSave.AccessToken);
            SaveToLocalSettings<string>(CreatedAtString, tokenToSave.CreatedAt);
            SaveToLocalSettings<string>(ScopeString, tokenToSave.Scope);
            SaveToLocalSettings<string>(TokenTypeString, tokenToSave.TokenType);
        }

        public static (bool wasLoadSuccessful, Auth tokenToReturn) LoadToken()
        {
            bool wasLoadSuccessful = true;
            Auth tokenToReturn = null;
            try
            {
                tokenToReturn.AccessToken = LoadFromLocalSettings<string>(AccessTokenString);
                tokenToReturn.CreatedAt = LoadFromLocalSettings<string>(CreatedAtString);
                tokenToReturn.Scope = LoadFromLocalSettings<string>(ScopeString);
                tokenToReturn.TokenType = LoadFromLocalSettings<string>(TokenTypeString);
            }

            catch
            {
                wasLoadSuccessful = false;
            }

            return (wasLoadSuccessful, tokenToReturn);

        }

        public static T LoadFromLocalSettings<T>(string settingString)
        {
            T settingsValue;
            settingsValue = (T)_localSettings.Values[settingString];
            return settingsValue;
        }


        public static void SaveToLocalSettings<T>(string settingString, T value)
        {
            _localSettings.Values[settingString] = value;
        }
    }
}
