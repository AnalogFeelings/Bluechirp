using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Helpers;

namespace Tooter.ViewModel
{
    class FederatedViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Federated Timeline";

        internal async override Task LoadFeedAsync()
        {
            base.TootTimelineCollection = await ClientHelper.Client.GetPublicTimeline();
        }
    }
}
