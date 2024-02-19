using Bluechirp.Library.Models.View;
using Bluechirp.Library.Models.View.Timelines;
using Bluechirp.Library.Services.Environment;
using Bluechirp.Library.Services.Interface;
using Bluechirp.Library.Services.Security;
using Bluechirp.Library.Services.Utility;
using Bluechirp.Services.Environment;
using Bluechirp.Services.Interface;
using Bluechirp.Services.Security;
using Bluechirp.Services.Utility;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bluechirp
{
    public partial class App
    {
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Gets the application's service provider.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the service provider is accessed before initializing.
        /// </exception>
        public static IServiceProvider ServiceProvider
        {
            get
            {
                IServiceProvider serviceProvider = (Current as App)._serviceProvider ??
                    throw new InvalidOperationException("Service provider was not initialized before accessing.");

                return serviceProvider;
            }
        }

        /// <summary>
        /// Initializes and configures the application's service provider.
        /// </summary>
        private void InitializeServices()
        {
            IServiceCollection collection = new ServiceCollection();

            // Add a dummy logger when on release mode, to prevent wasting cycles
            // on something the user is never going to see.
#if DEBUG
            collection.AddTransient<ILoggerService, LoggerService>();
#else
            collection.AddTransient<ILoggerService, DummyLoggerService>();
#endif
            collection.AddTransient<IEncryptionService, EncryptionService>();
            collection.AddSingleton<ICredentialService, CredentialService>();
            collection.AddSingleton<IAuthService, AuthService>();
            collection.AddSingleton<INavigationService, NavigationService>();
            collection.AddSingleton<ISettingsService, SettingsService>();
            collection.AddSingleton<IInstanceUtilityService, InstanceUtilityService>();
            collection.AddSingleton<IDispatcherService, DispatcherService>();

            // Add view models.
            collection.AddTransient<LoginViewModel>();
            collection.AddTransient<ShellViewModel>();
            collection.AddTransient<HomeTimelineViewModel>();

            _serviceProvider = collection.BuildServiceProvider(true);

            // Initialize singleton services that need it.
            _ = _serviceProvider.GetRequiredService<IDispatcherService>();
        }
    }
}
