using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using FitAndGym.Resources;
using FitAndGym.Infrastructure;

namespace FitAndGym.Converters
{
    public class SecondsToMinutesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return TimeSpan.FromSeconds((double)((int?)value).Value).ToMyFormat();
                //return String.Format("{0}:{1}{2}", ((int?)value).Value / 60, (((int?)value).Value % 60) < 10 ? "0" : "", ((int?)value).Value % 60);
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}