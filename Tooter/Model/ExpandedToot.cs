using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooter.Model
{
    public class ExpandedToot
    {
        public Status StoredToot { get; set; }
        
        public ExpandedToot(Status tootToStore)
        {
            StoredToot = tootToStore;
        }
    }
}
