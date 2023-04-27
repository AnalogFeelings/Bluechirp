using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooterLib.Enums;
using TooterLib.Helpers;
using TooterLib.Model;
using Tooter.Services;
using TooterLib.Services;

namespace Tooter.ViewModel
{
    class HomeViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Home";
        protected override TimelineType timelineType { get; set; } = TimelineType.Home;

        protected async override Task<MastodonList<Status>> GetTimeline()
        {
            return await ClientHelper.Client.GetHomeTimeline();
        }

        protected async override Task<MastodonList<Status>> GetNewerTimeline(ArrayOptions options)
        {
            return await ClientHelper.Client.GetHomeTimeline(options);
        }

        protected async override Task<MastodonList<Status>> GetOlderTimeline()
        {
            return await ClientHelper.Client.GetHomeTimeline(new ArrayOptions()
            {
                MaxId = nextPageMaxId
            });
        }
    }
}
