using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Helpers;
using Tooter.Model;

namespace Tooter.ViewModel
{
    class HomeViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Home";

        public HomeViewModel(): base()
        {
            
        }

        internal async override Task LoadFeedAsync()
        {
            TootTimelineCollection = await ClientHelper.Client.GetHomeTimeline();
        }
    }
}
