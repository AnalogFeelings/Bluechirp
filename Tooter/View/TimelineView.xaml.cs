using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public sealed partial class TimelineView : Page, INotifyPropertyChanged
    {

        private TimelineViewModelBase _viewModel;

        public TimelineViewModelBase ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                NotifyPropertyChanged();
            }
        }

        public TimelineView()
        {
            this.InitializeComponent();
        }



        public TimelineView(TimelineViewModelBase ViewModelToUse)
        {
            if (!this.IsLoaded)
            {
                this.InitializeComponent();
            }
            ViewModel = ViewModelToUse;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is TimelineViewModelBase newViewModel)
            {
                ViewModel = newViewModel;
            }
            else if (e.Parameter is Type ViewModelType)
            {
                ViewModel = Activator.CreateInstance(ViewModelType) as TimelineViewModelBase;
            }
            await ViewModel.LoadFeedAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
