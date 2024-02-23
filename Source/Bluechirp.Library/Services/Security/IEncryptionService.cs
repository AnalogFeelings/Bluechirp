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

using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Bluechirp.Library.Services.Security;

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