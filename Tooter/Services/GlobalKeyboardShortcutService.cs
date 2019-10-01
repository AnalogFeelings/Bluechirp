using EmojiDebugSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Dialogs;
using Tooter.Enums;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;

namespace Tooter.Services
{
    public static class GlobalKeyboardShortcutService
    {
        // Reference table: https://www.freepascal.org/docs-html/rtl/keyboard/kbdscancode.html
        const int ForwardSlashScanCode = 53;
        internal static event EventHandler<ShortcutType> GlobalShortcutPressed;

        internal static ShortcutMode CurrentShortcutMode { get; private set; }

        internal static void Initialize()
        {
            var coreWindow = CoreApplication.GetCurrentView().CoreWindow;
            coreWindow.KeyDown += CoreWindow_KeyDown;
            coreWindow.KeyUp += CoreWindow_KeyUp;
            coreWindow.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
        }

        private async static void Dispatcher_AcceleratorKeyActivated(Windows.UI.Core.CoreDispatcher sender, Windows.UI.Core.AcceleratorKeyEventArgs args)
        {
            if (args.KeyStatus.IsMenuKeyDown)
            {
                if (args.VirtualKey == Windows.System.VirtualKey.N)
                {
                    var tootDialog = new NewTootDialog();
                    try
                    {
                        await tootDialog.ShowAsync();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private static void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.G:
                    CurrentShortcutMode = ShortcutMode.Regular;
                    break;

                case Windows.System.VirtualKey.H:
                    if (CurrentShortcutMode == ShortcutMode.Global)
                    {
                        GlobalShortcutPressed?.Invoke(null, ShortcutType.Home);
                    }
                    break;

                case Windows.System.VirtualKey.L:
                    if (CurrentShortcutMode == ShortcutMode.Global)
                    {
                        GlobalShortcutPressed?.Invoke(null, ShortcutType.Local);
                    }
                    break;

                case Windows.System.VirtualKey.F:
                    if (CurrentShortcutMode == ShortcutMode.Global)
                    {
                        GlobalShortcutPressed?.Invoke(null, ShortcutType.Federated);
                    }
                    break;

                case Windows.System.VirtualKey.Shift:
                    CurrentShortcutMode = ShortcutMode.Regular;
                    break;
                
                default:
                    if (args.KeyStatus.ScanCode == ForwardSlashScanCode)
                    {
                        if (CurrentShortcutMode == ShortcutMode.Shift)
                        {
                            GlobalShortcutPressed?.Invoke(null, ShortcutType.Help);
                        }
                    }
                    
                    break;
            }

                        
            args.Handled = true;
        }

        private static void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.G)
            {
                CurrentShortcutMode = ShortcutMode.Global;
            }

            if (args.VirtualKey == Windows.System.VirtualKey.Shift)
            {
                CurrentShortcutMode = ShortcutMode.Shift;
                
            }

        }


    }
}
