using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    /// <summary>
    /// An object that represents plain Mastodon text.
    /// </summary>
    public class MastodonText : IMastodonContent
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
    }
}
