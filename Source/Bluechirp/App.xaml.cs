using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
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
            AppInstance mainInstance = AppInstance.FindOrRegisterForKey("main");

            if (!mainInstance.IsCurrent)
            {
                AppActivationArguments appArgs = AppInstance.GetCurrent().GetActivatedEventArgs();

                await mainInstance.RedirectActivationToAsync(appArgs);

                Process.GetCurrentProcess().Kill();

                return;
            }

            appWindow = new MainWindow();
            appWindow.Activate();

            appWindow.ShowSplash();
        }
    }
}
