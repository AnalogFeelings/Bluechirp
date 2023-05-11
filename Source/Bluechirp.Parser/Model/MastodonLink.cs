using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    /// <summary>
    /// An object that represents a Mastodon plain link.
    /// </summary>
    public class MastodonLink : IMastodonContent
    {
        /// <inheritdoc/>
        public string Content { get; set; }

        /// <inheritdoc/>
        public MastodonContentType ContentType => MastodonContentType.Link;

        public MastodonLink(string Content)
        {
            this.Content = Content;
        }
    }
}