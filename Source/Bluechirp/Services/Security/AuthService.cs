using Bluechirp.Library.Constants;
using Bluechirp.Library.Models;
using Bluechirp.Library.Services.Security;
using Mastonet;
using Mastonet.Entities;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Bluechirp.Services.Security;

/// <summary>
/// Implements an authentication helper service.
/// </summary>
/// <remarks>
/// Depends on <see cref="ICredentialService"/>.
/// </remarks>
internal class AuthService : IAuthService
{
    public MastodonClient Client { get; private set; }
    public ProfileCredentials CurrentProfile { get; private set; }

    private ICredentialService _credentialService;

    private AuthenticationClient _authClient { get; set; }
    private AppRegistration _appRegistration { get; set; }

    public event EventHandler OnAuthCompleted;

    public AuthService(ICredentialService credentialService)
    {
        _credentialService = credentialService;
    }

    /// <inheritdoc/>
    public async Task<string> CreateAuthUrlAsync(string instanceUrl)
    {
        _authClient = new AuthenticationClient(instanceUrl);
        _appRegistration = await _authClient.CreateApp(ApiConstants.APP_NAME,
            ApiConstants.APP_WEBSITE,
            ApiConstants.REDIRECT_URI,
            GranularScope.Write, GranularScope.Read, GranularScope.Follow);

        return _authClient.OAuthUrl(ApiConstants.REDIRECT_URI);
    }

    /// <inheritdoc/>
    /// <exception cref="UriFormatException">
    /// Thrown if the <paramref name="urlQuery"/> does not contain an OAuth code.
    /// </exception>
    public async Task CompleteAuthAsync(string urlQuery)
    {
        if (Client != null)
            return;

        string authCode = HttpUtility.ParseQueryString(urlQuery).Get("code");

        if (authCode == null)
            throw new UriFormatException("Could not acquire authentication code from provided URI.");

        Auth authObject = await _authClient.ConnectWithCode(authCode, ApiConstants.REDIRECT_URI);

        this.Client = new MastodonClient(_appRegistration.Instance, authObject.AccessToken);
        this.CurrentProfile = new ProfileCredentials
        {
            AuthToken = authObject,
            AppRegistration = _appRegistration
        };

        await _credentialService.StoreProfileDataAsync(CurrentProfile);

        OnAuthCompleted?.Invoke(null, EventArgs.Empty);
    }

    /// <inheritdoc/>
    public void LoadClientFromCredentials(ProfileCredentials credentials)
    {
        if (Client != null)
            return;

        this.Client = new MastodonClient(credentials.AppRegistration.Instance, credentials.AuthToken.AccessToken);
        this.CurrentProfile = credentials;
    }
}