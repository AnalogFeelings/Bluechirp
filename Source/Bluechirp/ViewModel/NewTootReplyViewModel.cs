using Bluechirp.Library.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using Mastonet.Entities;
using System.Threading.Tasks;

namespace Bluechirp.ViewModel
{
    partial class NewTootReplyViewModel : NewTootViewModel
    {
        [ObservableProperty]
        private Status _quoteToot;

        public NewTootReplyViewModel(Status quoteToot) : base()
        {
            QuoteToot = quoteToot;
        }

        protected async override Task SendTootAsync()
        {
            await ClientHelper.Client.PublishStatus(StatusContent, StatusVisibilty, QuoteToot.Id);
        }
    }
}
