using Mastonet.Entities;

namespace Bluechirp.Library.Models
{
    public record ProfileCredentials
    {
        public AppRegistration AppRegistration { get; }
        public Auth AuthToken { get; }
    }
}
