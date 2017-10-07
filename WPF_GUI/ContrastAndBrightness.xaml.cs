using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_GUI
{
    /// <summary>
    /// Interaction logic for ContrastAndBrightness.xaml
    /// </summary>
    public partial class ContrastAndBrightness : Window
    {
        private Image view;
        private MainWindow parent;

        public ContrastAndBrightness(Image _view, MainWindow _parent)
        {
            view = _view;
            parent = _parent;
            InitializeComponent();
        }

        public void ResetDialog()
        {
            contrastSlider.Value = 100;
            brightnessSlider.Value = 0;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Visibility = Visibility.Collapsed;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Visibility = Visibility.Collapsed;
        }

        private void ChangePreview()
        {
            float contrast = (float)(contrastSlider.Value / contrastSlider.Maximum * Const.Constants.MaxContrast);
            int brightness = (int)brightnessSlider.Value-255;
            parent.editor.applyContrastAndBrightness(contrast, brightness);
            parent.image.Source = parent.ConvertBitmapToImageSource(parent.editor.getPreview());
        }

        private void ContrastSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.IsLoaded)
            {
                ChangePreview();
            }
        }

        private void BrightnessSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.IsLoaded)
            {
                ChangePreview();
            }
        }
    }
}
