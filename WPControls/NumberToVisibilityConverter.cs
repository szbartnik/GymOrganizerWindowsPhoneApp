using System;
using System.Windows.Data;
using System.Windows;

namespace WPControls
{
    public class NumberToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int && (int)value > 0)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
