#region License Information (GPLv3)
// Bluechirp - A modern, native client for the Mastodon social media.
// Copyright (C) 2023 Analog Feelings and contributors.
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

using Bluechirp.Library.Constants;
using Bluechirp.Library.Services.Environment;
using System;
using Windows.Storage;

namespace Bluechirp.Services.Environment;

/// <summary>
/// Implements a thin interface around the WinRT settings API.
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
