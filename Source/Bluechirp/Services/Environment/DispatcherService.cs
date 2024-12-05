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
