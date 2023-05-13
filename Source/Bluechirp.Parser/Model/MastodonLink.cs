using System;
using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    /// <summary>
    /// An object that represents a Mastodon plain link.
    /// </summary>
    public class MastodonLink : IMastodonContent, IEquatable<MastodonLink>
    {
        /// <inheritdoc/>
        public string Content { get; set; }

        /// <inheritdoc/>
        public MastodonContentType ContentType => MastodonContentType.Link;

        public MastodonLink(string Content)
        {
            this.Content = Content;
        }

        public bool Equals(MastodonLink Other)
        {
            if (Other == null)
                return false;

            return this.Content == Other.Content && this.ContentType == Other.ContentType;
        }

        public override bool Equals(object Object)
        {
            return Equals(Object as MastodonLink);
        }

        public override int GetHashCode()
        {
            return (Content, ContentType).GetHashCode();
        }
    }
}