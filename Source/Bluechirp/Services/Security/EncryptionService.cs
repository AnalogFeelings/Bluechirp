using Bluechirp.Library.Services.Security;
using System;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace Bluechirp.Services.Security
{
    internal class EncryptionService : IEncryptionService
    {
        public const string USER_DESCRIPTOR = "LOCAL=user";
        public const string MACHINE_DESCRIPTOR = "LOCAL=machine";

        public async Task<IBuffer> EncryptStringAsync(string target, string descriptor)
        {
            DataProtectionProvider provider = new DataProtectionProvider(descriptor);
            IBuffer bufferString = CryptographicBuffer.ConvertStringToBinary(target, BinaryStringEncoding.Utf8);

            IBuffer encryptedBuffer = await provider.ProtectAsync(bufferString);

            return encryptedBuffer;
        }

        public async Task<string> DecryptBufferAsync(IBuffer target)
        {
            DataProtectionProvider provider = new DataProtectionProvider();
            IBuffer decryptedBuffer = await provider.UnprotectAsync(target);

            string decryptedString = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, decryptedBuffer);

            return decryptedString;
        }
    }
}
