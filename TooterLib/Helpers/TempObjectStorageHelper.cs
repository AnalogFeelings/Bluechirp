using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TooterLib.Helpers
{
    class TempObjectStorageHelper: LocalObjectStorageHelper
    {
        
        public TempObjectStorageHelper(): base()
        {
            base.Folder = ApplicationData.Current.TemporaryFolder;
            base.Settings = null;
        }
    }
}
