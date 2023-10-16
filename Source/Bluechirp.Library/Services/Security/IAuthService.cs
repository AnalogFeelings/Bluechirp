using Bluechirp.Library.Models;
using Mastonet;
using System;
using System.Threading.Tasks;

namespace Bluechirp.Library.Services.Security
{
    /// <summary>
    /// Defines a service interface for handling Mastodon OAuth processes.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// The current Mastodon client instance on use.
        /// </summary>
        public MastodonClient Client { get; }

        /// <summary>
        /// The current user profile being used.
        /// </summary>
        public ProfileCredentials CurrentProfile { get; }

        /// <summary>
        /// An event raised when the OAuth process is complete.
        /// </summary>
        public event EventHandler OnAuthCompleted;

        /// <summary>
        /// Creates an OAuth URL for the provided Mastodon instance.
        /// </summary>
        /// <param name="instanceUrl">The Mastodon instance's URL.</param>
        /// <returns>The generated OAuth URL.</returns>
        public Task<string> CreateAuthUrlAsync(string instanceUrl);

        /// <summary>
        /// Extracts the OAuth code from the returned URL and
        /// initializes the client.
        /// </summary>
        /// <param name="urlQuery">The URL query returned by the OAuth process.</param>
        public Task CompleteAuthAsync(string urlQuery);

        /// <summary>
        /// Initializes the Mastodon client from existing credentials.
        /// </summary>
        /// <param name="credentials">The credentials to use.</param>
        public void LoadClientFromCredentials(ProfileCredentials credentials);
    }
}
