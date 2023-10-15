using Bluechirp.Library.Services.Environment;
using Bluechirp.Library.Services.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using AppActivationArguments = Microsoft.Windows.AppLifecycle.AppActivationArguments;
using AppInstance = Microsoft.Windows.AppLifecycle.AppInstance;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace Bluechirp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private MainWindow appWindow;

        private ILoggerService _loggerService;
        private IAuthService _authService;

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
            AppActivationArguments appArgs = AppInstance.GetCurrent().GetActivatedEventArgs();

            if(appArgs.Kind == ExtendedActivationKind.Protocol)
            {
                if (appWindow == null)
                    Process.GetCurrentProcess().Kill();

                _loggerService.Log("Received protocol activation.", LogSeverity.Information);

                ProtocolActivatedEventArgs protocolArgs = appArgs.Data as ProtocolActivatedEventArgs;

                await _authService.CompleteAuthAsync(protocolArgs.Uri.Query);
            }
            else
            {
                await CheckSingleInstanceAsync(appArgs);

                appWindow = new MainWindow();
                appWindow.Activate();
                appWindow.ShowSplash();

                InitializeServices();

                _loggerService = ServiceProvider.GetRequiredService<ILoggerService>();
                _authService = ServiceProvider.GetRequiredService<IAuthService>();
            }
        }

        /// <summary>
        /// Checks if the current application is the main instance, if
        /// not, redirect and exit.
        /// </summary>
        /// <param name="appArgs">The activation parameters.</param>
        private async Task CheckSingleInstanceAsync(AppActivationArguments appArgs)
        {
            AppInstance mainInstance = AppInstance.FindOrRegisterForKey("main");

            if (!mainInstance.IsCurrent)
            {
                await mainInstance.RedirectActivationToAsync(appArgs);

                Process.GetCurrentProcess().Kill();

                return;
            }
        }
    }
}
