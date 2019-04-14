using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooter.ViewModel
{
    class FederatedViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Federated Timeline";
    }
}
