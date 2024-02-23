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

using Bluechirp.Library.Services.Environment;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Bluechirp.Services.Environment;

/// <summary>
/// A logger with a sink for the VS debugger.
/// </summary>
internal class LoggerService : ILoggerService
{
    public void Log(string message, LogSeverity severity, [CallerMemberName] string callerFunction = "")
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        string severityText = severity switch
        {
            LogSeverity.Information => "MSG",
            LogSeverity.Warning => "WRN",
            LogSeverity.Error => "ERR",
            _ => "???"
        };

        Debug.WriteLine($"[{severityText} :: {callerFunction}] {message}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogException(Exception exception)
    {
        this.Log(exception.Message, LogSeverity.Error);

        Debugger.Break();
    }
}

/// <summary>
/// A dummy logger that doesn't execute code.
/// </summary>
internal class DummyLoggerService : ILoggerService
{
    public void Log(string message, LogSeverity severity, [CallerMemberName] string callerFunction = "")
    {
        ;
    }

    public void LogException(Exception exception)
    {
        ;
    }
}