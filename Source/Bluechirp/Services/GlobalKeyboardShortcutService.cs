using System;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Bluechirp.Dialogs;
using Bluechirp.Enums;

namespace Bluechirp.Services
{
    /// <summary>
    /// Class that handles shortcuts on all views.
    /// </summary>
    public static class GlobalKeyboardShortcutService
    {
        // Reference table: https://www.freepascal.org/docs-html/rtl/keyboard/kbdscancode.html
        private const int _FORWARD_SLASH_SCAN_CODE = 53;

        internal static ShortcutMode CurrentShortcutMode { get; private set; } = ShortcutMode.Regular;
        internal static event EventHandler<ShortcutType> GlobalShortcutPressed;

        /// <summary>
        /// Adds the necessary event handlers to the <see cref="CoreWindow"/>.
        /// </summary>
        internal static void Initialize()
        {
            CoreWindow coreWindow = CoreApplication.GetCurrentView().CoreWindow;
            coreWindow.KeyDown += CoreWindow_KeyDown;
            coreWindow.KeyUp += CoreWindow_KeyUp;
            coreWindow.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
        }

        /// <summary>
        /// Handles accelerator key presses.
        /// </summary>
        /// <param name="Sender">Sender object.</param>
        /// <param name="Args">Event arguments.</param>
        private static async void Dispatcher_AcceleratorKeyActivated(CoreDispatcher Sender, AcceleratorKeyEventArgs Args)
        {
            if (Args.KeyStatus.IsMenuKeyDown)
            {
                if (Args.VirtualKey != VirtualKey.N) return;

                NewTootDialog tootDialog = new NewTootDialog();

                try
                {
                    await tootDialog.ShowAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Handler for key up events.
        /// </summary>
        /// <param name="Sender">Sender object.</param>
        /// <param name="Args">Event arguments.</param>
        private static void CoreWindow_KeyUp(CoreWindow Sender, KeyEventArgs Args)
        {
            switch (Args.VirtualKey)
            {
                case VirtualKey.G:
                    CurrentShortcutMode = ShortcutMode.Regular;
                    break;
                case VirtualKey.H:
                    if (CurrentShortcutMode == ShortcutMode.Global) GlobalShortcutPressed?.Invoke(null, ShortcutType.Home);
                    break;
                case VirtualKey.L:
                    if (CurrentShortcutMode == ShortcutMode.Global) GlobalShortcutPressed?.Invoke(null, ShortcutType.Local);
                    break;
                case VirtualKey.F:
                    if (CurrentShortcutMode == ShortcutMode.Global) GlobalShortcutPressed?.Invoke(null, ShortcutType.Federated);
                    break;
                case VirtualKey.Shift:
                    CurrentShortcutMode = ShortcutMode.Regular;
                    break;
                default:
                    if (Args.KeyStatus.ScanCode == _FORWARD_SLASH_SCAN_CODE)
                    {
                        if (CurrentShortcutMode == ShortcutMode.Shift)
                            GlobalShortcutPressed?.Invoke(null, ShortcutType.Help);
                    }
                    break;
            }

            Args.Handled = true;
        }

        /// <summary>
        /// Handler for key press events.
        /// </summary>
        /// <param name="Sender">Sender object.</param>
        /// <param name="Args">Event arguments.</param>
        private static void CoreWindow_KeyDown(CoreWindow Sender, KeyEventArgs Args)
        {
            switch (Args.VirtualKey)
            {
                case VirtualKey.G:
                    CurrentShortcutMode = ShortcutMode.Global;
                    break;
                case VirtualKey.Shift:
                    CurrentShortcutMode = ShortcutMode.Shift;
                    break;
            }
        }
    }
}