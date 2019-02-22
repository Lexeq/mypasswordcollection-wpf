using MPC.Model;
using MPC.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MPC.Converters
{
    [ValueConversion(typeof(PasswordItem), typeof(PasswordItemViewModel))]
    class PasswordItemToViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return ((PasswordItemViewModel)value).Item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return new PasswordItemViewModel((PasswordItem)value);

        }
    }
}
