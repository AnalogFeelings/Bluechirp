using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    public class MastodonHashtag : IMastodonContent
    {
        public string Content { get; set; }

        public MastodonContentType ContentType => MastodonContentType.Hashtag;

        public MastodonHashtag(string Content)
        {
            this.Content = Content;
        }
    }
}