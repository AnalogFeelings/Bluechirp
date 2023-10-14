using Mastonet.Entities;

namespace Bluechirp.Library.Models
{
    public record ProfileCredentials
    {
        public AppRegistration AppRegistration { get; init; }
        public Auth AuthToken { get; init; }
    }
}
