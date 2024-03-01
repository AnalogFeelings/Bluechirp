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
using System.Runtime.InteropServices;

namespace Bluechirp.Library.Interop;

public enum MonitorDpiType : int
{
    Effective = 0,
    Angular = 1,
    Raw = 2,
    Default = Effective
}

/// <summary>
/// Contains Win32 interop methods.
/// </summary>
public static partial class Native
{
    /// <summary>
    /// Queries the dots per inch (DPI) of a display.
    /// </summary>
    /// <param name="hmonitor">Handle of the monitor being queried.</param>
    /// <param name="dpiType">
    /// The type of DPI being queried. Possible values are from the <see cref="MonitorDpiType"/> enum.
    /// </param>
    /// <param name="dpiX">
    /// The value of the DPI along the X axis. This value always refers to the horizontal edge, even when the screen is rotated.
    /// </param>
    /// <param name="dpiY">
    /// The value of the DPI along the Y axis. This value always refers to the vertical edge, even when the screen is rotated.
    /// </param>
    /// <returns>Non-zero if unsuccessful.</returns>
    [LibraryImport("Shcore.dll", SetLastError = true)]
    public static partial int GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);
}
