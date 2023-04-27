using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Bluechirp.Model;

namespace Bluechirp.Converters
{
    public class TootDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is ExpandedToot expandedToot)
            {
                return (DataTemplate)App.Current.Resources["ExpandedTootDataTemplate"];
            }
            else if (item is Status threadToot)
            {
                return (DataTemplate)App.Current.Resources["ThreadTootDataTemplate"];
            }
            else
            {
                return base.SelectTemplateCore(item);
            }
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is ExpandedToot expandedToot)
            {
                return (DataTemplate)App.Current.Resources["ExpandedTootDataTemplate"];
            }
            else if (item is Status threadToot)
            {
                return (DataTemplate)App.Current.Resources["ThreadTootDataTemplate"];
            }
            else
            {
                return base.SelectTemplateCore(item);
            }
        }

    }
}
