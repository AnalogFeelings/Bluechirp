using Mastonet.Entities;
using MastoParserLib.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bluechirp.Model;
using BluechirpLib.Helpers;

namespace Bluechirp.ViewModel
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

        internal async Task AddInContextItems(Status tootToExpand)
        {
            _itemInContext = tootToExpand;
            var statusContext = await ClientHelper.Client.GetStatusContext(_itemInContext.Id);

            await UpdateVariablePropertiesOfStatus(tootToExpand);
            var expandedToot = new ExpandedToot(tootToExpand);

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

        private async Task UpdateVariablePropertiesOfStatus(Status tootToExpand)
        {
            var recentStatus = await ClientHelper.Client.GetStatus(tootToExpand.Id);

            tootToExpand.Favourited = recentStatus.Favourited;
            tootToExpand.Reblogged = recentStatus.Reblogged;
            tootToExpand.ReblogCount = recentStatus.ReblogCount;
            tootToExpand.FavouritesCount = recentStatus.FavouritesCount;
            
        }

        internal bool CheckIfExpandedToot(Status toot)
        {
            return toot.Id == _itemInContext.Id;
        }
    }
}
