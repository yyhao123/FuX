using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Demo.Windows.Controls.converter
{
    public class RadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().Equals(parameter?.ToString()) ?? false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = default(bool);
            int num;
            if (value is bool)
            {
                flag = (bool)value;
                num = 1;
            }
            else
            {
                num = 0;
            }
            if (((uint)num & (flag ? 1u : 0u)) == 0)
            {
                return Binding.DoNothing;
            }
            return parameter?.ToString();
        }
    }
}
