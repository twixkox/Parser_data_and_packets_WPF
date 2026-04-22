using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
