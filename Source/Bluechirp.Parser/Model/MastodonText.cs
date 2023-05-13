using System;
using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    /// <summary>
    /// An object that represents plain Mastodon text.
    /// </summary>
    public class MastodonText : IMastodonContent, IEquatable<MastodonText>
    {
        /// <inheritdoc/>
        public string Content { get; set; }
        public bool IsParagraph { get; set; }

        /// <inheritdoc/>
        public MastodonContentType ContentType => MastodonContentType.Text;

        public MastodonText(string Content, bool IsParagraph = false)
        {
            this.Content = Content;
            this.IsParagraph = IsParagraph;
        }

        public bool Equals(MastodonText Other)
        {
            if (Other == null)
                return false;

            return this.Content == Other.Content && this.ContentType == Other.ContentType && this.IsParagraph == Other.IsParagraph;
        }

        public override bool Equals(object Object)
        {
            return Equals(Object as MastodonText);
        }

        public override int GetHashCode()
        {
            return (Content, ContentType, IsParagraph).GetHashCode();
        }
    }
}
