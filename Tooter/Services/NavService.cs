using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Tooter.Services
{
    public sealed class NavService
    {
        private static Frame _frame = null;
        internal static NavService Instance = new NavService();
        private NavService() { }

        internal static void CreateInstance(Frame frame)
        {
            _frame = frame;
        }

        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }

        public void GoForward()
        {
            if (_frame.CanGoForward)
            {
                _frame.GoForward();
            }
        }


        public bool Navigate(Type viewModelType)
        {
            
            
            bool validNavigation = false;
            string viewName = viewModelType.Name.Replace("ViewModel", "");
            try
            {
                Type viewType = Type.GetType(viewName);
                validNavigation = _frame.Navigate(viewType);
            }
            catch (Exception)
            {

                
            }
           
            return validNavigation;
        }

        public bool Navigate(Type viewModelType, object parameter)
        {

            bool validNavigation = false;
            string viewName = viewModelType.Name.Replace("ViewModel", "");
            try
            {
                Type viewType = Type.GetType(viewName);
                validNavigation = _frame.Navigate(viewType, parameter);
            }
            catch (Exception)
            {


            }

            return validNavigation;
        }

        public bool Navigate(Type viewModelType, object parameter, NavigationTransitionInfo infoOverride)
        {

            bool validNavigation = false;
            string viewName = viewModelType.Name.Replace("ViewModel", "");
            try
            {
                Type viewType = Type.GetType(viewName);
                validNavigation = _frame.Navigate(viewType, parameter, infoOverride);
            }
            catch (Exception)
            {


            }

            return validNavigation;
        }

       

        public bool IsCurrentPageOfType(Type typeQuery)
        {
            return _frame.SourcePageType.Equals(typeQuery);
        }
    }
}
