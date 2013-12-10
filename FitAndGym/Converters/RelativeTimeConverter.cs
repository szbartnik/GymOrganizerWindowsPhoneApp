using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using FitAndGym.Resources;
using Microsoft.Phone.Controls;

namespace FitAndGym.Converters
{
    public class RelativeTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var converter = new FullViewDateTimeConverter();
            if (value != null)
            {
                TimeSpan difference = (DateTime)value - DateTime.Now;
                if (difference.Duration() > TimeSpan.FromHours(2))
                    return converter.Convert(value, targetType, parameter, culture);

                if (difference < TimeSpan.FromTicks(0))
                    return String.Format("{0} {1}", GetRelativeTime(difference.Duration(), true), AppResources.TimeConverterAgoWord);
                else
                    return String.Format("{0}{1}", AppResources.TimeConverterInWord, GetRelativeTime(difference.Duration(), false));
            }
            return "";
        }

        private string GetRelativeTime(TimeSpan difference, bool isPast)
        {
            int amountOfParts = 0;

            var str = new StringBuilder();
            if (difference.Days > 0)
            {
                ++amountOfParts;
                str.Append(" ")
                   .Append(difference.Days)
                   .Append(AppResources.TimeConverterDayWord);
            }

            if (difference.Hours > 0)
            {
                ++amountOfParts;
                str.Append(" ")
                   .Append(difference.Hours)
                   .Append(AppResources.TimeConverterHourWord);
            }

            if (difference.Minutes > 0 && amountOfParts++ < 2)
            {
                str.Append(" ")
                   .Append(difference.Minutes)
                   .Append(AppResources.TimeConverterMinuteWord);
            }

            if (difference.Seconds > 0 && amountOfParts++ < 2)
            {
                str.Append(" ")
                   .Append(difference.Seconds)
                   .Append(AppResources.TimeConverterSecondWord);
            }

            if(amountOfParts == 0)
                str.Append(" ")
                   .Append(1)
                   .Append(AppResources.TimeConverterSecondWord);

            return str.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
