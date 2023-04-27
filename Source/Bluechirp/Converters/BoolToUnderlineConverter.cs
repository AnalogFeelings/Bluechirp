using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml.Data;

namespace Bluechirp.Converters
{
    class BoolToUnderlineConverter : IValueConverter
    {
        const double roundedRatio = 0.15;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TextDecorations textDecorations = TextDecorations.None;

            if (value is bool shouldUnderlineShow)
            {
                textDecorations = shouldUnderlineShow ? TextDecorations.Underline : TextDecorations.None;
            }
            return textDecorations;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    
}
