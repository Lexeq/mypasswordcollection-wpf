using System;
using System.Globalization;
using System.Windows.Data;

namespace MPC
{
    class MultiBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            foreach (var item in values)
            {
                result |= (bool)item;
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
