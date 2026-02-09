using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Demo.Windows.Controls.filterDataGrid.Converters
{
    public class GridLineVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataGridGridLinesVisibility linesVisibility = (DataGridGridLinesVisibility)value;
            Console.WriteLine(linesVisibility.ToString());
            if (linesVisibility == DataGridGridLinesVisibility.All) return true; return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
