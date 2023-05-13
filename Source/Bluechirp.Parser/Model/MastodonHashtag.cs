using System;
using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    /// <summary>
    /// An object that represents a Mastodon hashtag link.
    /// </summary>
    public class MastodonHashtag : IMastodonContent, IEquatable<MastodonHashtag>
    {
        /// <inheritdoc/>
        public string Content { get; set; }

        /// <inheritdoc/>
        public MastodonContentType ContentType => MastodonContentType.Hashtag;

        public MastodonHashtag(string Content)
        {
            this.Content = Content;
        }

        public bool Equals(MastodonHashtag Other)
        {
            if (Other == null)
                return false;

            return this.Content == Other.Content && this.ContentType == Other.ContentType;
        }

        public override bool Equals(object Object)
        {
            return Equals(Object as MastodonHashtag);
        }

        public override int GetHashCode()
        {
            return (Content, ContentType).GetHashCode();
        }
    }
}