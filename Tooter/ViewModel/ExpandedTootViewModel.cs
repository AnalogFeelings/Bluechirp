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

        private ObservableCollection<Status> _contextTootItems;

        public ObservableCollection<Status> ContextTootItems
        {
            get { return _contextTootItems; }
            set { _contextTootItems = value;
                NotifyPropertyChanged();
            }
        }

        public ExpandedTootViewModel()
        {
            ContextTootItems = new ObservableCollection<Status>();
        }

        internal async Task AddInContextItems(ExpandedToot expandedToot)
        {
            _itemInContext = expandedToot;
            var statusContext = await ClientHelper.Client.GetStatusContext(_itemInContext.Id);
            foreach (var ancestor in statusContext.Ancestors)
            {
                ContextTootItems.Add(ancestor);
            }

            ContextTootItems.Add(expandedToot);

            foreach (var descendant in statusContext.Descendants)
            {
                ContextTootItems.Add(descendant);
            }
        }

        internal bool CheckIfExpandedToot(Status toot)
        {
            return toot.Id == _itemInContext.Id;
        }
    }
}
