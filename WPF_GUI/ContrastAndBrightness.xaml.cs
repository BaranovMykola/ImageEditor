using System;
using System.Collections.Generic;
using System.Linq;
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

        public ContrastAndBrightness(Image _view)
        {
            view = _view;
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
    }
}
