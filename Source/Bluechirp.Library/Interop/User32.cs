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
