using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    /// <summary>
    /// An object that represents a Mastodon emoji.
    /// </summary>
    public class MastodonEmoji : IMastodonContent
    {
        /// <inheritdoc/>
        public string Content { get; set; }

        /// <inheritdoc/>
        public MastodonContentType ContentType => MastodonContentType.Emoji;

        public MastodonEmoji(string Content)
        {
            this.Content = Content;
        }
    }
}