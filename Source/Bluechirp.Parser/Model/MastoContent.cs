namespace Bluechirp.Parser.Model
{
    public class MastoContent
    {
        public string Content { get; set; }
        public MastoContentType ContentType { get; set; }

        public MastoContent(string Content, MastoContentType ContentType)
        {
            this.Content = Content;
            this.ContentType = ContentType;
        }
    }
}
