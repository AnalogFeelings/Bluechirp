#region License Information (GPLv3)
// Bluechirp - A modern, native client for the Mastodon social media.
// Copyright (C) 2023-2024 Analog Feelings and contributors.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

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