using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FitAndGym.Resources;

namespace FitAndGym.Infrastructure
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        public LocalizedDescriptionAttribute(string resourceId)
            : base(GetStringFromResource(resourceId)) { }

        private static string GetStringFromResource(string resourceId)
        {
            return AppResources.ResourceManager.GetString(resourceId);
        }
    }
}
