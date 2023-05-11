using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    /// <summary>
    /// An object that represents a Mastodon user mention.
    /// </summary>
    public class MastodonMention : IMastodonContent
    {
        /// <inheritdoc/>
        public string Content { get; set; }

        /// <inheritdoc/>
        public MastodonContentType ContentType => MastodonContentType.Mention;

        public MastodonMention(string Content)
        {
            this.Content = Content;
        }
    }
}