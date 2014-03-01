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
        // Declared as Func<> in order to keep visual designer working (bug in VS? or my ignorance?! ;/)
        Func<DateTime, Dictionary<DateTime, int>> function = App.FitAndGymViewModel.GetNumberOfExercisesPerDayByMonth;

        public Dictionary<DateTime, int> Retrieve(DateTime month)
        {
            return function(month);
        }
    }
}
