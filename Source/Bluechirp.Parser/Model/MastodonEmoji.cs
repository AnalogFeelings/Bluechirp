using System;
using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    /// <summary>
    /// An object that represents a Mastodon emoji.
    /// </summary>
    public class MastodonEmoji : IMastodonContent, IEquatable<MastodonEmoji>
    {
        /// <inheritdoc/>
        public string Content { get; set; }

        /// <inheritdoc/>
        public MastodonContentType ContentType => MastodonContentType.Emoji;

        public MastodonEmoji(string Content)
        {
            this.Content = Content;
        }

        public bool Equals(MastodonEmoji Other)
        {
            if (Other == null)
                return false;

            return this.Content == Other.Content && this.ContentType == Other.ContentType;
        }

        public override bool Equals(object Object)
        {
            return Equals(Object as MastodonEmoji);
        }

        public override int GetHashCode()
        {
            return (Content, ContentType).GetHashCode();
        }
    }
}