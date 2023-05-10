using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    public class MastodonMention : IMastodonContent
    {
        public string Content { get; set; }

        public MastodonContentType ContentType => MastodonContentType.Mention;

        public MastodonMention(string Content)
        {
            this.Content = Content;
        }
    }
}