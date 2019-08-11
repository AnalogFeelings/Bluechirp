using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Tooter.Services
{
    public static class ErrorService
    {
        public static async Task ShowConnectionError()
        {
            var errorDialog = new ContentDialog();
            errorDialog.Title = "Connection Error";
            errorDialog.Content = "Please check your internet connection and try again later";
            errorDialog.CloseButtonText = "Ok";
            try
            {
                await errorDialog.ShowAsync();

            }
            catch (Exception)
            {
            }
        }
    }
}
