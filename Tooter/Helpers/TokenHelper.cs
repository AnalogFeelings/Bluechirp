using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Tooter.Helpers
{
    public class TokenHelper
    {

        ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        const string AccessTokenString = "accessToken";
        const string CreatedAtString = "createdAt";
        const string ScopeString = "scope";
        const string TokenTypeString = "tokenType";

        public void SaveToken(Auth tokenToSave)
        {
            SaveToLocalSettings<string>(AccessTokenString, tokenToSave.AccessToken);
            SaveToLocalSettings<string>(CreatedAtString, tokenToSave.CreatedAt);
            SaveToLocalSettings<string>(ScopeString, tokenToSave.Scope);
            SaveToLocalSettings<string>(TokenTypeString, tokenToSave.TokenType);
        }

        public (bool wasLoadSuccessful, Auth tokenToReturn) LoadToken()
        {
            bool wasLoadSuccessful = true;
            Auth tokenToReturn = null;
            try
            {

            }

            catch
            {
                wasLoadSuccessful = false;
            }

            var accessTokenLoadValues = LoadFromLocalSettings<string>(AccessTokenString);

            return (wasLoadSuccessful, tokenToReturn);

        }

        public T LoadFromLocalSettings<T>(string settingString)
        {
            T settingsValue = default;
            settingsValue = (T)_localSettings.Values[settingString];
            return settingsValue;
        }


        public void SaveToLocalSettings<T>(string settingString, T value)
        {
            _localSettings.Values[settingString] = value;
        }
    }
}
