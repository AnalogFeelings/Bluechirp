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
using System.Threading.Tasks;
using WinUIEx;

namespace Bluechirp
{
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
            ILoggerService logService = App.ServiceProvider.GetRequiredService<ILoggerService>();

            string lastProfile = settingsService.Get<string>(SettingsConstants.LAST_PROFILE_KEY);

            await credentialService.LoadProfileDataAsync();
            navService.TargetFrame = ContentFrame;

            ProfileCredentials credentials = credentialService.GetProfileData(lastProfile);

            // Hm. Let's check if there are profiles in storage.
            if (credentials == null)
            {
                ProfileCredentials defaultCredentials = credentialService.GetDefaultProfileData();

                // There aren't. Just show the login screen.
                if (defaultCredentials == null)
                {
                    logService.Log("No credentials found. Navigating to login page.", LogSeverity.Error);

                    navService.Navigate(PageType.Login, null, new DrillInNavigationTransitionInfo());
                }
                else
                {
                    // There is one! Use it.
                    LoadCredentialsAndOpenShell(defaultCredentials);
                }
            }
            else
            {
                LoadCredentialsAndOpenShell(credentials);
            }

            void LoadCredentialsAndOpenShell(ProfileCredentials credentials)
            {
                logService.Log("Credentials found. Attempting to initialize client...", LogSeverity.Information);

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
}
