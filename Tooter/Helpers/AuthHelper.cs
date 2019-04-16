using Mastonet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Services;
using Windows.System;

namespace Tooter.Helpers
{
    public sealed class AuthHelper
    {
        
        private static Lazy<AuthHelper> lazy =
            new Lazy<AuthHelper>(() => new AuthHelper());

        internal static AuthHelper Instance => lazy.Value;

        
        private AuthHelper(){}

        public async Task LoginAsync(string instanceUrl)
        {
            var authClient = new AuthenticationClient(instanceUrl);
            var appRegistration = await authClient.CreateApp("Tooter", Scope.Read | Scope.Write | Scope.Follow);
            var url = authClient.OAuthUrl();
            NavService.Instance.Navigate(typeof());
            await Launcher.LaunchUriAsync(new Uri(url));
        }

        internal Task FinishOAuth(string fullUri)
        {
            throw new NotImplementedException();
        }
    }
}
