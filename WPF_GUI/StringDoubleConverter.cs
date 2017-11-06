using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF_GUI
{
    public class StringDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double res;
            var readed = double.TryParse(value.ToString(), out res);
            if (readed)
            {
                return res;
            }
            return 1;
        }
    }
}