using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Enums;

namespace Tooter.Model
{
    internal class ShellMenuItem
    {
        public ShellMenuItemType ItemType { get; private set; }
        public string IconCharCode { get; set; }


        internal ShellMenuItem(ShellMenuItemType itemType, string iconCharCode)
        {
            ItemType = itemType;
            IconCharCode = iconCharCode;
        }
    }
}
