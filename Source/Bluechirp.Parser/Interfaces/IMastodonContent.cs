using Bluechirp.Parser.Model;

namespace Bluechirp.Parser.Interfaces
{
    public interface IMastodonContent
    {
        string Content { get; set; }
        MastodonContentType ContentType { get; }
    }
}
