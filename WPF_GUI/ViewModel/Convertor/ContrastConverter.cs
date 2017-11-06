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

            float input = (float)((double)value + 6.3);
            float contrast;
            if (input < 4.3)
            {
                contrast = (float)(Math.Pow(input, 1 / 3.0) / (3 * 1.64));
            }
            else if (input >= 4.3 && input < 6.3)
            {
                contrast = (float)((input * 0.332) - 1.0916);
            }
            else if (input >= 6.3 && input < 8.3)
            {
                contrast = (float)(input - 5.3);
            }
            else
            {
                contrast = (float)Math.Pow(input - 8.3 + 1.44, 3);
            }
            
            return contrast;
        }
    }
}
