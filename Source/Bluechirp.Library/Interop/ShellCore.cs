using System;
using System.Runtime.InteropServices;

namespace Bluechirp.Library.Interop
{
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
        [DllImport("Shcore.dll", SetLastError = true)]
        public static extern int GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);
    }
}
