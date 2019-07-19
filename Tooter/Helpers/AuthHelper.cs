using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Core;
using Tooter.Services;
using Tooter.View;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using System.Diagnostics;

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
            // Old code
            //_authClient = new AuthenticationClient(instanceUrl);
            //_appRegistration = await _authClient.CreateApp("Tooter", Scope.Read | Scope.Write | Scope.Follow);

            // New code
            _appRegistration = new AppRegistration();
            _appRegistration.ClientId = APIKeys.ClientID;
            _appRegistration.ClientSecret = APIKeys.ClientSecret;
            _appRegistration.Instance = instanceUrl;
            _appRegistration.Scope = Scope.Read | Scope.Write | Scope.Follow;
            _appRegistration.RedirectUri = APIKeys.RedirectUri;
            _authClient = new AuthenticationClient(_appRegistration);

            //SaveAppRegistration(_appRegistration);
            var url = _authClient.OAuthUrl(APIKeys.RedirectUri);
            NavService.Instance.Navigate(typeof(CodeView));
            await Launcher.LaunchUriAsync(new Uri(url));
        }

        private void SaveAppRegistration(AppRegistration appRegistration)
        {
            throw new NotImplementedException();
        }

        internal async Task FinishOAuth(string UriQuery)
        {
            try
            {
                WwwFormUrlDecoder urlParser = new WwwFormUrlDecoder(UriQuery);
                string authCode = urlParser.GetFirstValueByName("code");
                Debug.WriteLine(authCode);

                var auth = await _authClient.ConnectWithCode(authCode, APIKeys.RedirectUri);
                var client = new MastodonClient(_appRegistration, auth);
                ClientHelper.CreateClient(client);
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

        internal void SaveAccessToken(Auth auth)
        {
            TokenHelper.
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
