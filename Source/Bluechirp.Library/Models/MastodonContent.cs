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

using Bluechirp.Library.Enums;
using System;

namespace Bluechirp.Library.Models;

/// <summary>
/// A class that defines a type of content inside a toot.
/// </summary>
public class MastodonContent : IEquatable<MastodonContent>
{
    /// <summary>
    /// The text content of the object.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// The type of object.
    /// </summary>
    public MastodonContentType ContentType { get; init; }

    public bool Equals(MastodonContent other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Content == other.Content && ContentType == other.ContentType;
    }

    /// <inheritdoc/>
    public override bool Equals(object @object)
    {
        if (ReferenceEquals(null, @object))
        {
            return false;
        }

        if (ReferenceEquals(this, @object))
        {
            return true;
        }

        if (@object.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((MastodonContent)@object);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Content, (int)ContentType);
    }
}
