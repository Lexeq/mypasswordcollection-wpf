using System;
using System.Windows.Data;

namespace MPC
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        public  object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if(value is bool)
                return !(bool)value;
            throw new ArgumentException("value must be bool", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
