using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Data;
using WPControls;
using System.Windows.Media;
using System.Collections.Generic;

namespace FitAndGym.Converters
{
    public class CalendarMarkDatesRetriever : IMarkDatesRetriever
    {
        public Dictionary<DateTime, int> Retrieve(DateTime month)
        {
            return App.FitAndGymViewModel.GetNumberOfTrainingsPerDayByMonth(month);
        }
    }
}
