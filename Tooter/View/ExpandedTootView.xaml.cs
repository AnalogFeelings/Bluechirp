using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tooter.Model;
using Tooter.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Tooter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExpandedTootView : Page
    {
        public ExpandedTootView()
        {
            
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Status expandedToot)
            {
                await ViewModel.AddInContextItems(new ExpandedToot(expandedToot));

            }
        }

        private void ExpandedTootContextListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Status toot)
            {
                if (!ViewModel.CheckIfExpandedToot(toot))
                {
                    Frame.Navigate(typeof(ExpandedTootView), new ExpandedToot(toot));
                }
            }
        }
    }
}
