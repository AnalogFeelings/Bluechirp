using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Tooter.Converters
{
    class HalfConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double numToReturn = 0;
            if (value is double number)
            {
                numToReturn = number / 2;
            }

            return numToReturn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
