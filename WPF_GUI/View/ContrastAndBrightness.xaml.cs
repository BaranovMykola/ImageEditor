namespace WPF_GUI
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ContrastAndBrightness.xaml
    /// </summary>
    public partial class ContrastAndBrightness : Window
    {
        public ContrastAndBrightness()
        {
            InitializeComponent();
        }

        public event Action<float, int> PreviewChanged;

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
            int brightness = (int)brightnessSlider.Value - 255;
            PreviewChanged?.Invoke(contrast, brightness);
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
