using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Helpers;
using Windows.UI.Xaml.Data;

namespace Tooter.Converters
{
    class SimpleTimestampConverter : IValueConverter
    {
        const double roundedRatio = 0.15;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string timestampString = null;

            if (value is DateTime dateTimeToFormat)
            {
               timestampString = TimestampHelper.FormatTimestamp(dateTimeToFormat);
            }

            if (timestampString == null)
            {
                timestampString = string.Empty;
            }
            return timestampString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
