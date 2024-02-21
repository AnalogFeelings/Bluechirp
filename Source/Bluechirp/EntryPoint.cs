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