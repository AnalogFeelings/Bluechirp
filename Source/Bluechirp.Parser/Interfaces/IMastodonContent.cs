using Bluechirp.Parser.Model;

namespace Bluechirp.Parser.Interfaces
{
    /// <summary>
    /// An interface that defines a Mastodon toot object type.
    /// </summary>
    public interface IMastodonContent
    {
        /// <summary>
        /// The text content of the object.
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// The type of object.
        /// </summary>
        MastodonContentType ContentType { get; }
    }
}
