using Bluechirp.Library.Services.Environment;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AppActivationArguments = Microsoft.Windows.AppLifecycle.AppActivationArguments;
using AppInstance = Microsoft.Windows.AppLifecycle.AppInstance;

namespace Bluechirp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private MainWindow appWindow;

        private ILoggerService _loggerService;

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
            await CheckSingleInstanceAsync();

            appWindow = new MainWindow();
            appWindow.Activate();
            appWindow.ShowSplash();

            InitializeServices();

            _loggerService = ServiceProvider.GetRequiredService<ILoggerService>();

            _loggerService.Log("Hello, Fediverse!", LogSeverity.Information);
        }

        /// <summary>
        /// Checks if the current application is the main instance, if
        /// not, redirect and exit.
        /// </summary>
        private async Task CheckSingleInstanceAsync()
        {
            AppInstance mainInstance = AppInstance.FindOrRegisterForKey("main");

            if (!mainInstance.IsCurrent)
            {
                AppActivationArguments appArgs = AppInstance.GetCurrent().GetActivatedEventArgs();

                await mainInstance.RedirectActivationToAsync(appArgs);

                Process.GetCurrentProcess().Kill();

                return;
            }
        }
    }
}
