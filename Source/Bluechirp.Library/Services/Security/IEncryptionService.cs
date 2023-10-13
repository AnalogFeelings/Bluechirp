using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Bluechirp.Library.Services.Security
{
    /// <summary>
    /// Defines a service interface for encrypting and decrypting strings.
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Encrypts a string and stores it into a new <see cref="IBuffer"/>.
        /// </summary>
        /// <param name="target">The string to encrypt.</param>
        /// <param name="descriptor">The security descriptor.</param>
        /// <returns>A new <see cref="IBuffer"/> containing the encrypted data.</returns>
        public Task<IBuffer> EncryptStringAsync(string target, string descriptor);

        /// <summary>
        /// Decrypts an encrypted <see cref="IBuffer"/> into a string.
        /// </summary>
        /// <param name="target">The target buffer.</param>
        /// <returns>A string containing the decrypted data.</returns>
        public Task<string> DecryptBufferAsync(IBuffer target);
    }
}
