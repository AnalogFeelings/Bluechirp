using Bluechirp.Library.Models;
using System.Threading.Tasks;

namespace Bluechirp.Library.Services.Security
{
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
        public ProfileCredentials GetProfileData(string profileId);
    }
}
