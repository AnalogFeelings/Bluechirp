#region License Information (GPLv3)
// Bluechirp - A modern, native client for the Mastodon social media.
// Copyright (C) 2023-2024 Analog Feelings and contributors.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using System;

namespace Bluechirp.Library.Services.Environment;

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
    /// Retrieves the value of the setting from the disk.
    /// </summary>
    /// <typeparam name="T">The type to cast the setting value to.</typeparam>
    /// <param name="key">The setting's key.</param>
    /// <returns>The setting value, or its default if <see langword="null"/>.</returns>
    public T Get<T>(string key);

    /// <summary>
    /// Sets the value of a setting and saves it to the disk.
    /// </summary>
    /// <typeparam name="T">The type of the value to set.</typeparam>
    /// <param name="key">The setting's key.</param>
    /// <param name="value">The value to set.</param>
    public void Set<T>(string key, T value);
}
