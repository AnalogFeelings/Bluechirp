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
using System.Threading.Tasks;

namespace Bluechirp.Library.Services.Security;

/// <summary>
/// Defines a service interface for storing user credentials.
/// </summary>
public interface ICredentialService
{
    /// <summary>
    /// Loads the user credentials from encrypted storage.
    /// </summary>
    public Task LoadProfileDataAsync();

    /// <summary>
    /// Stores a user's credentials into the encrypted storage.
    /// </summary>
    /// <param name="credentials">The credentials to store.</param>
    public Task StoreProfileDataAsync(ProfileCredentials credentials);

    /// <summary>
    /// Removes a user's credentials from storage and memory.
    /// </summary>
    /// <param name="credentials">The credentials to remove.</param>
    public Task RemoveProfileDataAsync(ProfileCredentials credentials);

    /// <summary>
    /// Gets a user's credentials from memory.
    /// </summary>
    /// <param name="profileId">The profile ID.</param>
    /// <returns>The user's credentials object.</returns>
    public ProfileCredentials? GetProfileData(string profileId);

    /// <summary>
    /// Gets the first profile in memory, for emergency purposes.
    /// </summary>
    /// <returns>A user's credentials object.</returns>
    public ProfileCredentials? GetDefaultProfileData();
}
