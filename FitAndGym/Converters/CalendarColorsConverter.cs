using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Data;
using WPControls;
using System.Windows.Media;

namespace FitAndGym.Converters
{
    public class CalendarColorsConverter : IDateToBrushConverter
    {
        public Brush Convert(DateTime dateTime, bool isSelected, bool isMarked, Brush defaultValue, BrushType brushType)
        {
            return defaultValue;
        }
    }
}
