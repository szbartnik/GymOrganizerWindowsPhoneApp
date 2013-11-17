using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FitAndGym.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                Type type = value.GetType();
                string name = Enum.GetName(type, value);
                if (name != null)
                {
                    FieldInfo field = type.GetField(name);
                    if (field != null)
                    {
                        DescriptionAttribute attr =
                               Attribute.GetCustomAttribute(field,
                                 typeof(DescriptionAttribute)) as DescriptionAttribute;
                        if (attr != null)
                        {
                            return attr.Description;
                        }
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
