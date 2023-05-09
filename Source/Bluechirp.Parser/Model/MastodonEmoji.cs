using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    public class MastodonEmoji : IMastodonContent
    {
        public string Content { get; set; }
        public MastodonContentType ContentType => MastodonContentType.Emoji;

        public MastodonEmoji(string Content)
        {
            this.Content = Content;
        }
    }
}