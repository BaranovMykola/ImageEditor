namespace WPF_GUI.ViewModel.Convertor
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    internal class ContrastConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 1;
            }

            float input = (float)(double)value;
            float contrast;
            if (input >= 0)
            {
                contrast = (float)(Math.Pow(input, 2) + 1);
            }
            else
            {
                contrast = (float)(1 / (Math.Pow(input, 2) + 1));
            }

            Console.WriteLine(contrast);
            return contrast;
        }
    }
}
