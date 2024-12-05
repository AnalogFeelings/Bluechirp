#region License Information (GPLv3)
// Bluechirp - A modern, native client for the Mastodon social media.
// Copyright (C) 2023 Analog Feelings and contributors.
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
using Bluechirp.Library.Interop;
using Bluechirp.Library.Services.Environment;
using Bluechirp.Library.Services.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using WinUIEx;
using AppActivationArguments = Microsoft.Windows.AppLifecycle.AppActivationArguments;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace Bluechirp;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private MainWindow _appWindow;

    private MatchaLogger _loggerService;
    private IAuthService _authService;
    private IDispatcherService _dispatcherService;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        _appWindow = new MainWindow();
        _appWindow.Activate();
        _appWindow.ShowSplash();

        InitializeServices();

        _loggerService = ServiceProvider.GetRequiredService<MatchaLogger>();
        _authService = ServiceProvider.GetRequiredService<IAuthService>();
        _dispatcherService = ServiceProvider.GetRequiredService<IDispatcherService>();

        await _loggerService.LogAsync(LogSeverity.Information, "Bluechirp is initializing.");

        await _appWindow.CheckLoginAndNavigateAsync();
    }

    /// <summary>
    /// Invoked when the application is activated.
    /// </summary>
    /// <param name="args">Details about the activation request.</param>
    public async Task OnActivated(AppActivationArguments args)
    {
        await _dispatcherService.EnqueueAsync(() =>
        {
            // BUG WORKAROUND: microsoft-ui-xaml#7595
            // Window.Activate() does not bring window to foreground.
            // https://github.com/microsoft/microsoft-ui-xaml/issues/7595
            Native.ShowWindow(_appWindow.GetWindowHandle(), 0x9);
        });

        if (args.Kind == ExtendedActivationKind.Protocol)
        {
            if (_appWindow == null)
                Process.GetCurrentProcess().Kill();

            await _loggerService.LogAsync(LogSeverity.Information, "Received protocol activation.");

            ProtocolActivatedEventArgs protocolArgs = args.Data as ProtocolActivatedEventArgs;

            await _dispatcherService.EnqueueAsync(async () =>
            {
                await _authService.CompleteAuthAsync(protocolArgs.Uri.Query);
            });

        }
    }
}
