using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Bluechirp.Converters;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Bluechirp.ViewModel;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Bluechirp.Dialogs
{
    public sealed partial class NewTootDialog : ContentDialog
    {

        private NewTootViewModel _viewModel = new NewTootViewModel();

        public NewTootDialog()
        {
            this.InitializeComponent();
        }

        public NewTootDialog(string staticAvatarUrl)
        {
            this.InitializeComponent();
            UserAvatar.ProfilePicture = new BitmapImage(new Uri(staticAvatarUrl));

        }

        

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        
    }
}
