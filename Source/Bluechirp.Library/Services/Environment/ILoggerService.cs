using System;
using System.Runtime.CompilerServices;

namespace Bluechirp.Library.Services.Environment
{
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
}
