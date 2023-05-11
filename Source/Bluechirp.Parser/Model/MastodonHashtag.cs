using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    /// <summary>
    /// An object that represents a Mastodon hashtag link.
    /// </summary>
    public class MastodonHashtag : IMastodonContent
    {
        /// <inheritdoc/>
        public string Content { get; set; }

        /// <inheritdoc/>
        public MastodonContentType ContentType => MastodonContentType.Hashtag;

        public MastodonHashtag(string Content)
        {
            this.Content = Content;
        }
    }
}