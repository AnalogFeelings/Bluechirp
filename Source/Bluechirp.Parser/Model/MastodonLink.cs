using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    public class MastodonLink : IMastodonContent
    {
        public string Content { get; set; }

        public MastodonContentType ContentType => MastodonContentType.Link;

        public MastodonLink(string Content)
        {
            this.Content = Content;
        }
    }
}