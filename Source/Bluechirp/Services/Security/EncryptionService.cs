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

using Bluechirp.Library.Services.Security;
using System;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace Bluechirp.Services.Security;

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
        DataProtectionProvider provider = new DataProtectionProvider();

        InMemoryRandomAccessStream inputData = new InMemoryRandomAccessStream();
        InMemoryRandomAccessStream unprotectedData = new InMemoryRandomAccessStream();

        IOutputStream outputStream = inputData.GetOutputStreamAt(0);
        DataWriter writer = new DataWriter(outputStream);

        writer.WriteBuffer(target);
        await writer.StoreAsync();
        await outputStream.FlushAsync();

        IInputStream source = inputData.GetInputStreamAt(0);
        IOutputStream dest = unprotectedData.GetOutputStreamAt(0);

        await provider.UnprotectStreamAsync(source, dest);
        await dest.FlushAsync();

        DataReader reader = new DataReader(unprotectedData.GetInputStreamAt(0));
        await reader.LoadAsync((uint)unprotectedData.Size);

        IBuffer buffUnprotectedData = reader.ReadBuffer((uint)unprotectedData.Size);
        string decryptedString = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffUnprotectedData);

        return decryptedString;
    }
}