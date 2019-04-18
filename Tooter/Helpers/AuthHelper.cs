using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Services;
using Tooter.View;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Tooter.Helpers
{
    public sealed class AuthHelper
    {
        private AppRegistration _appRegistration = null;
        private AuthenticationClient _authClient = null;
        private static Lazy<AuthHelper> lazy =
            new Lazy<AuthHelper>(() => new AuthHelper());
        internal MastodonClient _client = null;
        internal static AuthHelper Instance => lazy.Value;


        private AuthHelper() { }

        public async Task LoginAsync(string instanceUrl)
        {
            _authClient = new AuthenticationClient(instanceUrl);
            _appRegistration = await _authClient.CreateApp("Tooter", Scope.Read | Scope.Write | Scope.Follow);
            //SaveAppRegistration(_appRegistration);
            var url = _authClient.OAuthUrl();
            NavService.Instance.Navigate(typeof(CodeView));
            await Launcher.LaunchUriAsync(new Uri(url));
        }

        private void SaveAppRegistration(AppRegistration appRegistration)
        {
            throw new NotImplementedException();
        }

        internal Task FinishOAuth(string fullUri)
        {
            throw new NotImplementedException();
        }

        internal void SaveAccessToken(Auth auth)
        {
            throw new NotImplementedException();
        }

        internal async Task TryConnectWithCode(string code)
        {
            try
            {
                var auth = await _authClient.ConnectWithCode(code);
                //SaveAccessToken(auth);
                var _client = new MastodonClient(_appRegistration, auth);
                ClientHelper.CreateClient(_client);
                NavService.Instance.Navigate(typeof(ShellView));
            }
            catch (Exception)
            {
                var errorDialog = new ContentDialog()
                {
                    Title = "Problem connecting with code",
                    Content = "Check if connection is working and code is correct",
                    CloseButtonText = "Ok"
                };
                await errorDialog.ShowAsync();
            }
        }

        public AppRegistration TryGetAppRegistration()
        {
            return _appRegistration;
        }
    }
}
