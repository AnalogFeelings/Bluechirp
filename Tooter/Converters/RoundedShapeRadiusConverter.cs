using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Tooter.Converters
{
    class RoundedShapeRadiusConverter : IValueConverter
    {
        const double roundedRatio = 0.15;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double radiusToUse = 0;
            if (value is double width)
            {
                if (!double.IsNaN(width))
                {
                    radiusToUse = width * roundedRatio;
                }
            }
            return new CornerRadius(radiusToUse);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
