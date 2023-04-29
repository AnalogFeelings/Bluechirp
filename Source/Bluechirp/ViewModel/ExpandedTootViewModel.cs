using Bluechirp.Library.Helpers;
using Bluechirp.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using Mastonet.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Bluechirp.ViewModel
{
    class ExpandedTootViewModel : ObservableObject
    {
        private Status _itemInContext = null;

        public ObservableCollection<Status> ContextTootItems;

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
