using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tooter.Helpers;

namespace Tooter.Model
{
    public class ExpandedToot: Status
    {
        public Status TootRef { get; private set; }
        public ExpandedToot(Status expandedToot)
        {
            ObjectManipulationHelper.CopyPropertiesTo(expandedToot, this);
            TootRef = expandedToot;
        }

       



    }
}
