using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPControls
{
    public interface IMarkDatesRetriever
    {
        Dictionary<DateTime, int> Retrieve(DateTime month);
    }
}
