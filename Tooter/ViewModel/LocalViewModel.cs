using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Helpers;
using Tooter.Model;

namespace Tooter.ViewModel
{
    class LocalViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Local Timeline";

        internal async override Task LoadFeedAsync()
        {
            base.TootTimelineCollection = await ClientHelper.Client.GetPublicTimeline(local: true);
        }
    }
}
