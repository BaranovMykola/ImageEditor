namespace WPF_GUI.ViewModel.Convertor
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Const;

    public class BoolToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var swich = (bool)value;
            return swich ? Constants.AnchorMatrixItemThickness : Constants.DefaultMatrixItemThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
