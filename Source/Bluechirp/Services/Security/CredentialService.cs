using Bluechirp.Library.Extensions;
using Bluechirp.Library.Models;
using Bluechirp.Library.Services.Security;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Bluechirp.Services.Security
{
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

        private const string CREDENTIAL_FILENAME = "profiles.ebjson";

        public CredentialService(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        /// <inheritdoc/>
        public async Task LoadProfileDataAsync()
        {
            StorageFile profilesFile = await GetProfilesFileAsync();
            ulong fileSize = await profilesFile.GetFileSizeAsync();

            IBuffer encryptedProfiles = await FileIO.ReadBufferAsync(profilesFile);
            string decryptedProfiles = await _encryptionService.DecryptBufferAsync(encryptedProfiles);

            _profileCredentials = JsonSerializer.Deserialize<Dictionary<string, ProfileCredentials>>(decryptedProfiles);
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
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the credentials don't exist in memory.
        /// </exception>
        public ProfileCredentials GetProfileData(string profileId)
        {
            if (!_profileCredentials.ContainsKey(profileId))
                throw new KeyNotFoundException("Could not find the requested profile data.");

            return _profileCredentials[profileId];
        }

        /// <summary>
        /// Creates or opens the profiles storage file.
        /// </summary>
        /// <returns>The profile storage file handle.</returns>
        private async Task<StorageFile> GetProfilesFileAsync()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile profilesFile = await folder.CreateFileAsync(CREDENTIAL_FILENAME, CreationCollisionOption.OpenIfExists);

            return profilesFile;
        }

        /// <summary>
        /// Writes the profile credentials to disk in encrypted form.
        /// </summary>
        private async Task WriteProfilesAsync()
        {
            StorageFile profilesFile = await GetProfilesFileAsync();
            string serializedProfiles = JsonSerializer.Serialize(profilesFile);
            IBuffer encryptedProfiles = await _encryptionService.EncryptStringAsync(serializedProfiles, EncryptionService.USER_DESCRIPTOR);

            await FileIO.WriteBufferAsync(profilesFile, encryptedProfiles);
        }
    }
}
