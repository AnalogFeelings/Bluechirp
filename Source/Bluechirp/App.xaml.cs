using Bluechirp.Library.Services.Environment;
using Bluechirp.Library.Services.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using WinUIEx;
using AppActivationArguments = Microsoft.Windows.AppLifecycle.AppActivationArguments;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace Bluechirp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, uint Msg);

        private MainWindow appWindow;

        private ILoggerService _loggerService;
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
            appWindow = new MainWindow();
            appWindow.Activate();
            appWindow.ShowSplash();

            InitializeServices();

            _loggerService = ServiceProvider.GetRequiredService<ILoggerService>();
            _authService = ServiceProvider.GetRequiredService<IAuthService>();
            _dispatcherService = ServiceProvider.GetRequiredService<IDispatcherService>();

            _loggerService.Log("Bluechirp is initializing.", LogSeverity.Information);

            await appWindow.CheckLoginAndNavigateAsync();
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
                ShowWindow(appWindow.GetWindowHandle(), 0x9);
            });

            if (args.Kind == ExtendedActivationKind.Protocol)
            {
                if (appWindow == null)
                    Process.GetCurrentProcess().Kill();

                _loggerService.Log("Received protocol activation.", LogSeverity.Information);

                ProtocolActivatedEventArgs protocolArgs = args.Data as ProtocolActivatedEventArgs;

                await _dispatcherService.EnqueueAsync(async () =>
                {
                    await _authService.CompleteAuthAsync(protocolArgs.Uri.Query);
                });

            }
        }
    }
}
