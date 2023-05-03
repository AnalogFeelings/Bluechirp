namespace Bluechirp.Parser.Model
{
    public class MastoText : MastoContent
    {
        public bool IsParagraph { get; set; }

        public MastoText(string Content, bool IsParagraph = false) : base(Content, MastoContentType.Text)
        {
            this.IsParagraph = IsParagraph;
        }
    }
}
