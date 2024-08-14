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
using AnalogFeelings.Matcha.Sinks.Debugger;
using AnalogFeelings.Matcha.Sinks.File;
using Bluechirp.Library.Constants;
using Bluechirp.Library.Models.View;
using Bluechirp.Library.Models.View.Navigation;
using Bluechirp.Library.Models.View.Timelines;
using Bluechirp.Library.Services.Environment;
using Bluechirp.Library.Services.Interface;
using Bluechirp.Library.Services.Mastodon;
using Bluechirp.Library.Services.Security;
using Bluechirp.Library.Services.Utility;
using Bluechirp.Services.Environment;
using Bluechirp.Services.Interface;
using Bluechirp.Services.Mastodon;
using Bluechirp.Services.Security;
using Bluechirp.Services.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.IO;
using Windows.Storage;

namespace Bluechirp;

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
            App currentApp = (Current as App)!;

            if(currentApp._serviceProvider == null)
                throw new InvalidOperationException("Service provider was not initialized before accessing.");

            return currentApp._serviceProvider;
        }
    }

    /// <summary>
    /// Initializes and configures the application's service provider.
    /// </summary>
    private void InitializeServices()
    {
        IServiceCollection collection = new ServiceCollection();

        LogSeverity filterLevel;

#if DEBUG
        filterLevel = LogSeverity.Debug;
#else
        filterLevel = LogSeverity.Information;
#endif

        FileSinkConfig fileConfig = new FileSinkConfig()
        {
            SeverityFilterLevel = filterLevel,
            FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, AppConstants.LOG_FOLDER),
            Overwrite = true
        };
        DebuggerSinkConfig debuggerConfig = new DebuggerSinkConfig()
        {
            SeverityFilterLevel = filterLevel
        };

        FileSink fileSink = new FileSink()
        {
            Config = fileConfig
        };
        DebuggerSink debuggerSink = new DebuggerSink()
        {
            Config = debuggerConfig
        };

        MatchaLogger logger = new MatchaLogger(fileSink, debuggerSink);

        // Add services.
        collection.AddSingleton(logger);
        collection.AddTransient<IEncryptionService, EncryptionService>();
        collection.AddTransient<IMastodonTextParserService, MastodonTextParserService>();
        collection.AddSingleton<ICredentialService, CredentialService>();
        collection.AddSingleton<IAuthService, AuthService>();
        collection.AddSingleton<INavigationService, NavigationService>();
        collection.AddSingleton<ISettingsService, SettingsService>();
        collection.AddSingleton<IInstanceUtilityService, InstanceUtilityService>();
        collection.AddSingleton<IDispatcherService, DispatcherService>();
        collection.AddSingleton<IInfoService, InfoService>();

        // Add view models.
        collection.AddTransient<LoginViewModel>();
        collection.AddTransient<ShellViewModel>();
        collection.AddTransient<SettingsViewModel>();
        collection.AddTransient<HomeTimelineViewModel>();

        _serviceProvider = collection.BuildServiceProvider(true);

        // Initialize singleton services that need it.
        _ = _serviceProvider.GetRequiredService<IDispatcherService>();
    }
}
