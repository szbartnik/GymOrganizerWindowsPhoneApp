using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitAndGym.Infrastructure
{
    public static class TimeSpanToMinutesAndSeconds
    {
        public static string ToMyFormat(this TimeSpan ts)
        {
            return String.Format("{0}:{1}", ts.Hours > 0 ? ts.Hours * 60 + ts.Minutes : ts.Minutes, ts.ToString("ss"));
        }
    }
}
