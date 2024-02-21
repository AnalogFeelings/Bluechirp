using Bluechirp.Library.Services.Environment;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using System;
using System.Threading.Tasks;

namespace Bluechirp.Services.Environment;

/// <summary>
/// Implements a thin interface around the WinRT dispatcher queue API.
/// </summary>
/// <remarks>
/// Used to run code in the UI thread. Must be initialized in it.
/// </remarks>
internal class DispatcherService : IDispatcherService
{
    private readonly DispatcherQueue _dispatcherQueue;

    public DispatcherService()
        => _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    /// <inheritdoc/>
    public Task EnqueueAsync(Action function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        return _dispatcherQueue.EnqueueAsync(function, priority);
    }

    /// <inheritdoc/>
    public Task EnqueueAsync(Func<Task> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        return _dispatcherQueue.EnqueueAsync(function, priority);
    }

    /// <inheritdoc/>
    public Task<T> EnqueueAsync<T>(Func<T> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        return _dispatcherQueue.EnqueueAsync(function, priority);
    }

    /// <inheritdoc/>
    public Task<T> EnqueueAsync<T>(Func<Task<T>> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        return _dispatcherQueue.EnqueueAsync(function, priority);
    }
}
