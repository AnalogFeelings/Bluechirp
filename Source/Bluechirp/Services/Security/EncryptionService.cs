using Bluechirp.Library.Services.Security;
using System;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace Bluechirp.Services.Security
{
    /// <summary>
    /// Implements a data encryption service.
    /// </summary>
    internal class EncryptionService : IEncryptionService
    {
        public const string USER_DESCRIPTOR = "LOCAL=user";
        public const string MACHINE_DESCRIPTOR = "LOCAL=machine";

        /// <inheritdoc/>
        public async Task<IBuffer> EncryptStringAsync(string target, string descriptor)
        {
            DataProtectionProvider provider = new DataProtectionProvider(descriptor);
            IBuffer bufferString = CryptographicBuffer.ConvertStringToBinary(target, BinaryStringEncoding.Utf8);

            IBuffer encryptedBuffer = await provider.ProtectAsync(bufferString);

            return encryptedBuffer;
        }

        /// <inheritdoc/>
        public async Task<string> DecryptBufferAsync(IBuffer target)
        {
            DataProtectionProvider Provider = new DataProtectionProvider();

            InMemoryRandomAccessStream inputData = new InMemoryRandomAccessStream();
            InMemoryRandomAccessStream unprotectedData = new InMemoryRandomAccessStream();

            IOutputStream outputStream = inputData.GetOutputStreamAt(0);
            DataWriter writer = new DataWriter(outputStream);

            writer.WriteBuffer(target);
            await writer.StoreAsync();
            await outputStream.FlushAsync();

            IInputStream source = inputData.GetInputStreamAt(0);
            IOutputStream dest = unprotectedData.GetOutputStreamAt(0);

            await Provider.UnprotectStreamAsync(source, dest);
            await dest.FlushAsync();

            DataReader reader = new DataReader(unprotectedData.GetInputStreamAt(0));
            await reader.LoadAsync((uint)unprotectedData.Size);

            IBuffer buffUnprotectedData = reader.ReadBuffer((uint)unprotectedData.Size);
            string decryptedString = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffUnprotectedData);

            return decryptedString;
        }
    }
}
