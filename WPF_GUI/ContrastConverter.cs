using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_GUI
{
    class ContrastConverter : IValueConverter
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
            float input = (float)(double) value;
            float contrast;
            if (input >= 0)
            {
                contrast = (float) (Math.Pow(input, 2) + 1);
            }
            else
            {
                contrast = (float) (1/ (Math.Pow(input, 2) + 1));
            }
            Console.WriteLine(contrast);
            return contrast;
        }
    }
}
