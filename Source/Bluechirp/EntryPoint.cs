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

using Bluechirp.Library.Helpers;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Threading;
using System.Threading.Tasks;
using WinRT;

namespace Bluechirp;

/// <summary>
/// Custom entry point for Bluechirp, needed for rich activation 
/// and single-instancing.
/// </summary>
internal class EntryPoint
{
    [STAThread]
    private static async Task Main(string[] args)
    {
        ComWrappersSupport.InitializeComWrappers();
        bool isMainInstance = await CheckSingleInstanceAsync();

        if (!isMainInstance)
            return;

        Application.Start((x) =>
        {
            DispatcherQueue threadQueue = DispatcherQueue.GetForCurrentThread();
            DispatcherQueueSynchronizationContext syncContext = new DispatcherQueueSynchronizationContext(threadQueue);

            SynchronizationContext.SetSynchronizationContext(syncContext);

            _ = new App();
        });
    }

    /// <summary>
    /// Check if there is one instance of Bluechirp running.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the current instance is the only one running.
    /// </returns>
    private static async Task<bool> CheckSingleInstanceAsync()
    {
        bool isMainInstance = true;
        AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();
        AppInstance keyInstance = AppInstance.FindOrRegisterForKey("bluechirp-main");

        if (keyInstance.IsCurrent)
        {
            keyInstance.Activated += KeyInstance_Activated;
        }
        else
        {
            isMainInstance = false;
            await keyInstance.RedirectActivationToAsync(args);
        }

        return isMainInstance;
    }

    /// <summary>
    /// Ran when the application is activated.
    /// </summary>
    private static void KeyInstance_Activated(object sender, AppActivationArguments e)
    {
        if (Application.Current is App app)
        {
            AsyncHelper.RunSync(() => app.OnActivated(e));
        }
    }
}