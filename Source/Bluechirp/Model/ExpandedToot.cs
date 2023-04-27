using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bluechirp.Helpers;

namespace Bluechirp.Model
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
