using Bluechirp.Library.Constants;
using Bluechirp.Library.Services.Environment;
using System;
using Windows.Storage;

namespace Bluechirp.Services.Environment
{
    /// <summary>
    /// Implements a thin interface around the 
    /// WinRT settings API.
    /// </summary>
    internal class SettingsService : ISettingsService
    {
        /// <inheritdoc/>
        public event EventHandler<string> OnSettingChanged;

        /// <inheritdoc/>
        public T Get<T>(string key)
        {
            object result = ApplicationData.Current.LocalSettings.Values[key];

            return result == null ? (T)SettingsConstants.Defaults[key] : (T)result;
        }

        /// <inheritdoc/>
        public void Set<T>(string key, T value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = value;

            OnSettingChanged?.Invoke(null, key);
        }
    }
}
