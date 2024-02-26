using System;

namespace Bluechirp.Library.Services.Environment;

/// <summary>
/// Defines a service interface for application and system information.
/// </summary>
public interface IInfoService
{
    /// <summary>
    /// The application's version.
    /// </summary>
    Version AppVersion { get; }
}
