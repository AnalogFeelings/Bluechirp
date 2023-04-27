using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BluechirpLib.Helpers;

namespace Bluechirp.ViewModel
{
    class NewTootReplyViewModel : NewTootViewModel
    {
        private Status _quoteToot;

        public Status QuoteToot
        {
            get { return _quoteToot; }
            set
            {
                _quoteToot = value;
                NotifyPropertyChanged();
            }
        }


        public NewTootReplyViewModel(Status quoteToot) : base()
        {
            QuoteToot = quoteToot;
        }
        protected async override Task SendNewToot()
        {
            await ClientHelper.Client.PublishStatus(StatusContent, StatusVisibilty, QuoteToot.Id);
        }
    }
}
