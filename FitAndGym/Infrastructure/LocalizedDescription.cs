using System;
using System.ComponentModel;
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
