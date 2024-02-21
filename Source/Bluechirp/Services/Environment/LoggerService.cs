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