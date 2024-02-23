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

using Bluechirp.Library.Enums;
using Bluechirp.Library.Services.Interface;
using Bluechirp.Library.Services.Security;
using Bluechirp.Library.Services.Utility;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using Windows.System;

namespace Bluechirp.Library.Models.View;

/// <summary>
/// Implements a view model for a login page.
/// </summary>
public partial class LoginViewModel : ObservableObject, IDisposable
{
    [ObservableProperty]
    private string _instanceUrl;

    private IInstanceUtilityService _instanceUtils;
    private INavigationService _navigationService;
    private IAuthService _authService;

    public LoginViewModel(INavigationService navigationService,
                          IAuthService authService,
                          IInstanceUtilityService instanceUtils)
    {
        _instanceUtils = instanceUtils;
        _navigationService = navigationService;
        _authService = authService;

        _authService.OnAuthCompleted += AuthService_OnAuthCompleted;
    }

    /// <summary>
    /// Logs in to the instance defined in <see cref="ViewModel.LoginViewModel.InstanceUrl"/>.
    /// </summary>
    [RelayCommand]
    private async Task LoginAsync()
    {
        if (_instanceUtils.CheckInstanceName(InstanceUrl))
        {
            string rawUrl = await _authService.CreateAuthUrlAsync(InstanceUrl);
            Uri oauthUri = new Uri(rawUrl);

            await Launcher.LaunchUriAsync(oauthUri);
        }
        else
        {
            InstanceUrl = string.Empty;
        }
    }

    /// <summary>
    /// Opens a browser to the sign up page of the instance defined in <see cref="ViewModel.LoginViewModel.InstanceUrl"/>.
    /// </summary>
    [RelayCommand]
    private async Task SignUpAsync()
    {
        if (_instanceUtils.CheckInstanceName(InstanceUrl))
        {
            // Concat the URIs safely.
            Uri baseUri = new Uri($"https://{InstanceUrl}");

            await Launcher.LaunchUriAsync(new Uri(baseUri, "auth/sign_up"));
        }
        else
        {
            InstanceUrl = string.Empty;
        }
    }

    private void AuthService_OnAuthCompleted(object sender, EventArgs e)
    {
        _navigationService.Navigate(PageType.Shell);
    }

    public void Dispose()
    {
        _authService.OnAuthCompleted -= AuthService_OnAuthCompleted;
    }
}