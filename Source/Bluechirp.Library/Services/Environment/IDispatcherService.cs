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

using Microsoft.UI.Dispatching;
using System;
using System.Threading.Tasks;

namespace Bluechirp.Library.Services.Environment;

/// <summary>
/// Defines a service interface for action enqueuing.
/// </summary>
public interface IDispatcherService
{
    /// <summary>
    /// Enqueues an <see cref="Action"/> with a priority level.
    /// </summary>
    /// <param name="function">The action to enqueue.</param>
    /// <param name="priority">The action's priority in the queue.</param>
    public Task EnqueueAsync(Action function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal);

    /// <summary>
    /// Enqueues a <see cref="Task"/> with a priority level.
    /// </summary>
    /// <param name="function">The task to enqueue.</param>
    /// <param name="priority">The task's priority in the queue.</param>
    /// <returns>A task that acts as a proxy for the one retured by the function.</returns>
    public Task EnqueueAsync(Func<Task> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal);

    /// <summary>
    /// Enqueues a function of return type <typeparamref name="T"/> with a priority.
    /// </summary>
    /// <typeparam name="T">The return type of the function.</typeparam>
    /// <param name="function">The function to enqueue.</param>
    /// <param name="priority">The function's priority in the queue.</param>
    /// <returns>The return value of the function.</returns>
    public Task<T> EnqueueAsync<T>(Func<T> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal);

    /// <summary>
    /// Enqueues a <see cref="Task{TResult}"/> with a priority.
    /// </summary>
    /// <typeparam name="T">The return type of the task.</typeparam>
    /// <param name="function">The task to enqueue.</param>
    /// <param name="priority">The task's priority in the queue.</param>
    /// <returns>A task that acts as a proxy for the one returned by the function.</returns>
    public Task<T> EnqueueAsync<T>(Func<Task<T>> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal);
}