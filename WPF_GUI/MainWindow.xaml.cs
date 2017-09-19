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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(@"D:\Studying\Programming\ImageEditor\WPF_GUI\fox.jpg", UriKind.Absolute);
            b.EndInit();
            //image.Source = b;
        }

        private void openFile(object sender, RoutedEventArgs e)
        {
            var b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(@"D:\Studying\Programming\ImageEditor\WPF_GUI\fox.jpg", UriKind.Absolute);
            b.EndInit();
            image.Source = b;
        }
    }
}
