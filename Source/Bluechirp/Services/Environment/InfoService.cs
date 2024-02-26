using Bluechirp.Library.Services.Environment;
using System;
using Windows.ApplicationModel;

namespace Bluechirp.Services.Environment;

/// <summary>
/// Implements an application and system information helper service.
/// </summary>
internal class InfoService : IInfoService
{
    /// <inheritdoc/>
    public Version AppVersion { get; init; }

    public InfoService()
    {
        Package package = Package.Current;
        PackageVersion version = package.Id.Version;

        this.AppVersion = new Version(version.Major, version.Minor, version.Build);
    }
}
