using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooterLib.Core;
using TooterLib.Services;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TooterLib.Helpers
{
    public sealed class AuthHelper
    {
        private AppRegistration _appRegistration = null;
        private AuthenticationClient _authClient = null;
        private static Lazy<AuthHelper> lazy =
            new Lazy<AuthHelper>(() => new AuthHelper());
        public MastodonClient _client = null;
        public static AuthHelper Instance => lazy.Value;
        public static event EventHandler AuthCompleted;

        private AuthHelper() { }


        public async Task LoginAsync(string instanceUrl)
        {
            // Login to any instance code
            _authClient = new AuthenticationClient(instanceUrl);
            _appRegistration = await _authClient.CreateApp(APIConstants.AppName,
                                                           Scope.Read | Scope.Write | Scope.Follow,
                                                           APIConstants.AppWebsite,
                                                           APIConstants.RedirectUri);

            var url = _authClient.OAuthUrl(APIConstants.RedirectUri);

            await Launcher.LaunchUriAsync(new Uri(url));
        }


        public async Task FinishOAuth(string UriQuery)
        {
            try
            {
                WwwFormUrlDecoder urlParser = new WwwFormUrlDecoder(UriQuery);
                string authCode = urlParser.GetFirstValueByName("code");
                Debug.WriteLine(authCode);

                var auth = await _authClient.ConnectWithCode(authCode, APIConstants.RedirectUri);
                var client = new MastodonClient(_appRegistration, auth);
                ClientHelper.CreateClient(client);
                var currentUser = await ClientHelper.Client.GetCurrentUser();

                string clientProfileID = $"{_appRegistration.Instance}{currentUser.Id}";

                ClientHelper.SetLoadedProfile(clientProfileID);
                ClientDataHelper.SetLastUsedProfile(clientProfileID);
                await ClientDataHelper.StoreClientData(clientProfileID, auth, _appRegistration);

                AuthCompleted?.Invoke(null, EventArgs.Empty);
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
    }
}
