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
            errorDialog.Content = "Please check your internet connection then restart the app to try again";
            errorDialog.CloseButtonText = "Ok";
            try
            {
                await errorDialog.ShowAsync();

            }
            catch (Exception)
            {
            }
        }

        public static async Task ShowInstanceUrlFormattingError()
        {
            var errorDialog = new ContentDialog();
            errorDialog.Title = "Formatting error";
            errorDialog.Content = "Make sure the instance name is written as part1.part2";
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
