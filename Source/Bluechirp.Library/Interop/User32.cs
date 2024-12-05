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

using System;
using System.Runtime.InteropServices;

namespace Bluechirp.Library.Interop;

public static partial class Native
{
    /// <summary>
    /// Sets the specified window's show state.
    /// </summary>
    /// <param name="hWnd">A handle to the window.</param>
    /// <param name="msg">Controls how the window is to be shown.</param>
    /// <returns>If the window was previously hidden, the return value is zero.</returns>
    [LibraryImport("user32.dll")]
    public static partial int ShowWindow(IntPtr hWnd, uint msg);
}
