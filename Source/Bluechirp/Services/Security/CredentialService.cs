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

using Bluechirp.Library.Models;
using Bluechirp.Library.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Bluechirp.Services.Security;

/// <summary>
/// Implements a user credential storage service.
/// </summary>
/// <remarks>
/// Depends on <see cref="IEncryptionService"/>.
/// </remarks>
internal class CredentialService : ICredentialService
{
    private IEncryptionService _encryptionService;

    private Dictionary<string, ProfileCredentials> _profileCredentials;

    private const string _CREDENTIAL_FILENAME = "profiles.ebjson";

    public CredentialService(IEncryptionService encryptionService)
    {
        _encryptionService = encryptionService;
    }

    /// <inheritdoc/>
    public async Task LoadProfileDataAsync()
    {
        StorageFile profilesFile = await GetProfilesFileAsync();

        IBuffer encryptedProfiles = await FileIO.ReadBufferAsync(profilesFile);
        string decryptedProfiles = await _encryptionService.DecryptBufferAsync(encryptedProfiles);

        if (string.IsNullOrEmpty(decryptedProfiles))
        {
            _profileCredentials = new Dictionary<string, ProfileCredentials>();
        }
        else
        {
            _profileCredentials = JsonSerializer.Deserialize<Dictionary<string, ProfileCredentials>>(decryptedProfiles);
        }
    }

    /// <inheritdoc/>
    public async Task StoreProfileDataAsync(ProfileCredentials credentials)
    {
        string profileId = credentials.AppRegistration.Instance + credentials.AppRegistration.Id;

        if (_profileCredentials.ContainsKey(profileId))
            return;

        _profileCredentials.Add(profileId, credentials);

        await WriteProfilesAsync();
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an attempt to remove credentials that aren't stored is made.
    /// </exception>
    public async Task RemoveProfileDataAsync(ProfileCredentials credentials)
    {
        string profileId = credentials.AppRegistration.Instance + credentials.AppRegistration.Id;

        if (!_profileCredentials.ContainsKey(profileId))
            throw new InvalidOperationException("Attempted to remove credentials that don't exist in storage.");

        _profileCredentials.Remove(profileId);

        await WriteProfilesAsync();
    }

    /// <inheritdoc/>
    public ProfileCredentials? GetProfileData(string profileId)
    {
        return _profileCredentials.GetValueOrDefault(profileId);
    }

    /// <inheritdoc/>
    public ProfileCredentials? GetDefaultProfileData()
    {
        if (_profileCredentials.Count == 0)
            return null;

        return _profileCredentials.First().Value;
    }

    /// <summary>
    /// Creates or opens the profiles storage file.
    /// </summary>
    /// <returns>The profile storage file handle.</returns>
    private async Task<StorageFile> GetProfilesFileAsync()
    {
        StorageFolder folder = ApplicationData.Current.LocalFolder;
        StorageFile profilesFile = await folder.CreateFileAsync(_CREDENTIAL_FILENAME, CreationCollisionOption.OpenIfExists);

        return profilesFile;
    }

    /// <summary>
    /// Writes the profile credentials to disk in encrypted form.
    /// </summary>
    private async Task WriteProfilesAsync()
    {
        StorageFile profilesFile = await GetProfilesFileAsync();
        string serializedProfiles = JsonSerializer.Serialize(_profileCredentials);
        IBuffer encryptedProfiles = await _encryptionService.EncryptStringAsync(serializedProfiles, EncryptionService.USER_DESCRIPTOR);

        await FileIO.WriteBufferAsync(profilesFile, encryptedProfiles);
    }
}
