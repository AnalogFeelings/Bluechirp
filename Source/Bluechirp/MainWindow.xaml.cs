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

using AnalogFeelings.Matcha;
using AnalogFeelings.Matcha.Enums;
using Bluechirp.Library.Constants;
using Bluechirp.Library.Enums;
using Bluechirp.Library.Models;
using Bluechirp.Library.Services.Environment;
using Bluechirp.Library.Services.Interface;
using Bluechirp.Library.Services.Security;
using Bluechirp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media.Animation;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WinUIEx;

namespace Bluechirp;

public sealed partial class MainWindow : WindowEx
{
    public static new MainWindow Current;

    public MainWindow()
    {
        this.InitializeComponent();

        Current = this;

        this.MinWidth = 380;
        this.MinHeight = 570;

        AppWindow.Title = "Bluechirp";
        AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
        AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
    }

    /// <summary>
    /// Checks if there are credentials stored in the disk.
    /// If none are found, the content frame will navigate to <see cref="LoginPage"/>.
    /// Otherwise, it will navigate to <see cref="ShellPage"/>.
    /// </summary>
    public async Task CheckLoginAndNavigateAsync()
    {
        ISettingsService settingsService = App.ServiceProvider.GetRequiredService<ISettingsService>();
        ICredentialService credentialService = App.ServiceProvider.GetRequiredService<ICredentialService>();
        IAuthService authService = App.ServiceProvider.GetRequiredService<IAuthService>();
        INavigationService navService = App.ServiceProvider.GetRequiredService<INavigationService>();
        MatchaLogger logService = App.ServiceProvider.GetRequiredService<MatchaLogger>();

        string lastProfile = settingsService.Get<string>(SettingsConstants.LAST_PROFILE_KEY);

        await credentialService.LoadProfileDataAsync();
        navService.TargetFrame = ContentFrame;

        ProfileCredentials? credentials = credentialService.GetProfileData(lastProfile);

        // Hm. Let's check if there are profiles in storage.
        if (credentials == null)
        {
            ProfileCredentials? defaultCredentials = credentialService.GetDefaultProfileData();

            // There aren't. Just show the login screen.
            if (defaultCredentials == null)
            {
                await logService.LogAsync(LogSeverity.Error, "No credentials found. Navigating to login page.");

                navService.Navigate(PageType.Login, null, new DrillInNavigationTransitionInfo());
            }
            else
            {
                // There is one! Use it.
                await LoadCredentialsAndOpenShell(defaultCredentials);
            }
        }
        else
        {
            await LoadCredentialsAndOpenShell(credentials);
        }

        async Task LoadCredentialsAndOpenShell(ProfileCredentials credentials)
        {
            await logService.LogAsync(LogSeverity.Information, "Credentials found. Attempting to initialize client...");

            authService.LoadClientFromCredentials(credentials);

            navService.Navigate(PageType.Shell, null, new DrillInNavigationTransitionInfo());
        }
    }

    /// <summary>
    /// Shows the splash screen.
    /// </summary>
    public void ShowSplash()
    {
        ContentFrame.Navigate(typeof(SplashScreenPage));
    }
}
