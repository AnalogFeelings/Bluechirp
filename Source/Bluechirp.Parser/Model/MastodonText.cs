using Bluechirp.Parser.Interfaces;

namespace Bluechirp.Parser.Model
{
    public class MastodonText : IMastodonContent
    {
        public string Content { get; set; }
        public bool IsParagraph { get; set; }

        public MastodonContentType ContentType => MastodonContentType.Text;

        public MastodonText(string Content, bool IsParagraph = false)
        {
            this.Content = Content;
            this.IsParagraph = IsParagraph;
        }
    }
}
