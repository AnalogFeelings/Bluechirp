using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Services;
using TooterLib.Enums;
using TooterLib.Helpers;
using TooterLib.Model;
using TooterLib.Services;

namespace Tooter.ViewModel
{
    class FederatedViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Federated Timeline";
        protected override TimelineType timelineType { get; set; } = TimelineType.Federated;
      

        protected async override Task<MastodonList<Status>> GetNewerTimeline(ArrayOptions options)
        {
            return await ClientHelper.Client.GetPublicTimeline(options);
        }

        protected async override Task<MastodonList<Status>> GetOlderTimeline()
        {
            return await ClientHelper.Client.GetPublicTimeline(new ArrayOptions()
            {
                MaxId = nextPageMaxId
            });
        }

        protected async override Task<MastodonList<Status>> GetTimeline()
        {
            return await ClientHelper.Client.GetPublicTimeline();
        }
    }
}
 