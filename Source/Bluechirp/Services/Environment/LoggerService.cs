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

using Bluechirp.Library.Constants;
using Bluechirp.Library.Helpers;
using Bluechirp.Library.Services.Environment;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace Bluechirp.Services.Environment;

/// <summary>
/// A logger with a sink for the VS debugger.
/// </summary>
internal class LoggerService : ILoggerService, IDisposable
{
    private readonly StreamWriter _logFileWriter;
    private readonly object _logLock = new object();

    public LoggerService()
    {
        StorageFolder folder = ApplicationData.Current.LocalFolder;
        StorageFile logFile = AsyncHelper.RunSync(() => folder.CreateFileAsync(AppConstants.LOG_FILE, CreationCollisionOption.ReplaceExisting).AsTask());
        Stream stream = AsyncHelper.RunSync(() => logFile.OpenStreamForWriteAsync());
        
        _logFileWriter = new StreamWriter(stream)
        {
            AutoFlush = true
        };
    }

    /// <inheritdoc/>
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
        string assembledMessage = $"[{severityText} :: {callerFunction}] {message}";

        Debug.WriteLine(assembledMessage);

        lock(_logLock)
        {
            _logFileWriter.WriteLine(assembledMessage);
        }
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogException(Exception exception)
    {
        this.Log(exception.ToString(), LogSeverity.Error);

        Debugger.Break();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _logFileWriter?.Dispose();
    }
}
