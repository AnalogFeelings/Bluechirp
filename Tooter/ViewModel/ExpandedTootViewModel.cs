using Mastonet.Entities;
using MastoParserLib.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Model;
using TooterLib.Helpers;

namespace Tooter.ViewModel
{
    class ExpandedTootViewModel: Notifier
    {
        private Status _itemInContext = null;

        private ObservableCollection<object> _contextTootItems;

        public ObservableCollection<object> ContextTootItems
        {
            get { return _contextTootItems; }
            set { _contextTootItems = value;
                NotifyPropertyChanged();
            }
        }

        public ExpandedTootViewModel()
        {
            ContextTootItems = new ObservableCollection<object>();
        }

        internal async Task AddInContextItems(ExpandedToot expandedToot)
        {
            _itemInContext = expandedToot.StoredToot;
            var statusContext = await ClientHelper.Client.GetStatusContext(_itemInContext.Id);
            

        }

    }
}
