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
