using System;

namespace Bluechirp.Library.Services.Environment
{
    /// <summary>
    /// Defines a service interface for getting
    /// and setting setting keys.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// An event raised when a setting is changed.
        /// </summary>
        public event EventHandler<string> OnSettingChanged;

        /// <summary>
        /// Retrieves the value of the setting from
        /// the disk.
        /// </summary>
        /// <typeparam name="T">The type to cast the setting value to.</typeparam>
        /// <param name="key">The setting's key.</param>
        /// <returns>The setting value, or its default if <see langword="null"/>.</returns>
        public T Get<T>(string key);

        /// <summary>
        /// Sets the value of a setting and saves
        /// it to the disk.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="key">The setting's key.</param>
        /// <param name="value">The value to set.</param>
        public void Set<T>(string key, T value);
    }
}
