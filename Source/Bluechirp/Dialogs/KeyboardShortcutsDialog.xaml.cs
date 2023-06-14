using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Bluechirp.Services;
using Microsoft.Extensions.DependencyInjection;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Bluechirp.Dialogs
{
    public sealed partial class KeyboardShortcutsDialog : ContentDialog
    {
        private CacheService _cacheService;

        public KeyboardShortcutsDialog()
        {
            this.InitializeComponent();

            _cacheService = App.Services.GetRequiredService<CacheService>();

            MarkdownContent.Text = _cacheService.KeyboardShortuctsContent;
            this.Opened += KeyboardShortcutsDialog_Opened;
        }

        private void KeyboardShortcutsDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            this.Focus(FocusState.Programmatic);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
