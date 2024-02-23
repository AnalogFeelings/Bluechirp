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

using System;
using System.Runtime.CompilerServices;

namespace Bluechirp.Library.Services.Environment;

/// <summary>
/// The severity of the log message.
/// </summary>
public enum LogSeverity
{
    Information,
    Warning,
    Error
}

/// <summary>
/// Defines a service interface for logging debug messages.
/// </summary>
public interface ILoggerService
{
    /// <summary>
    /// Logs a message to an external sink.
    /// </summary>
    /// <param name="message">The message to output.</param>
    /// <param name="severity">The severity of the message.</param>
    public void Log(string message, LogSeverity severity, [CallerMemberName] string callerFunction = "");

    /// <summary>
    /// Logs an exception to an external sink.
    /// </summary>
    /// <param name="exception">The exception to output.</param>
    public void LogException(Exception exception);
}