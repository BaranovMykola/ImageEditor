namespace WPF_GUI.ViewModel.Convertor
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class StringDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value?.ToString();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double res;
            var readed = double.TryParse(value.ToString(), out res);
            return readed ? res : 1;
        }
    }
}