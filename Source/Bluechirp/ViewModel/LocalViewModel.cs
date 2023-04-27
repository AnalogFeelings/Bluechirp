using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BluechirpLib.Enums;
using BluechirpLib.Helpers;
using BluechirpLib.Model;
using Bluechirp.Services;
using BluechirpLib.Services;

namespace Bluechirp.ViewModel
{
    class LocalViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Local Timeline";
        protected override TimelineType timelineType { get; set; } = TimelineType.Local;


        protected async override Task<MastodonList<Status>> GetNewerTimeline(ArrayOptions options)
        {
            return await ClientHelper.Client.GetPublicTimeline(options, true);
        }

        protected async override Task<MastodonList<Status>> GetOlderTimeline()
        {
            return await ClientHelper.Client.GetPublicTimeline(new ArrayOptions()
            {
                MaxId = nextPageMaxId
            }, local:true);
        }

        protected async override Task<MastodonList<Status>> GetTimeline()
        {
            return await ClientHelper.Client.GetPublicTimeline(local:true);
        }
    }
}
