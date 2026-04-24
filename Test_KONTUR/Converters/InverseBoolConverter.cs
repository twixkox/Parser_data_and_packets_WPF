using System;
using System.Globalization;

namespace Test_KONTUR.Converters
{
    public class InverseBoolConverter
    {
        public object Convert(object value, Type targetTupe, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return true;
        }
    }
}
