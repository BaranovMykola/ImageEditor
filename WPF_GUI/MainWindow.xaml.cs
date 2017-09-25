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
using Microsoft.Win32;
using WPF_GUI.ImageContainer;

namespace WPF_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageStorage openedImage = new ImageStorage();

        public MainWindow()
        {
            InitializeComponent();
            openedImage.LockLeft = lockLeftButton;
            openedImage.LockRight = lockrightButton;
            openedImage.UnlockAll = unlockLeftRightButton;
        }

        private void openFile(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Multiselect = true;
            openDialog.ShowDialog();
            var pathes = openDialog.FileNames;

            openedImage.LoadImages(pathes);
            image.Source = openedImage.Current;

        }

        private void LeftButton_OnClick(object sender, RoutedEventArgs e)
        {
     
                image.Source = openedImage.Prev;
  
        }

        private void RightButton_OnClick(object sender, RoutedEventArgs e)
        {
 
                image.Source = openedImage.Next;
      
        }

        private void lockLeftButton()
        {
            leftButton.IsEnabled = false;
            BitmapImage leftGrayIcon = new BitmapImage();
            leftGrayIcon.BeginInit();
            leftGrayIcon.UriSource = new Uri(@"D:\Studying\Programming\ImageEditor\WPF_GUI\icons\left_gray.ico", UriKind.Absolute);
            leftGrayIcon.EndInit();
            leftIco.Source = leftGrayIcon;
        }

        private void lockrightButton()
        {
            rightButton.IsEnabled = false;
            BitmapImage rightGrayIcon = new BitmapImage();
            rightGrayIcon.BeginInit();
            rightGrayIcon.UriSource = new Uri(@"D:\Studying\Programming\ImageEditor\WPF_GUI\icons\right_gray.ico", UriKind.Absolute);
            rightGrayIcon.EndInit();
            rightIco.Source = rightGrayIcon;
        }

        private void unlockLeftRightButton()
        {
            rightButton.IsEnabled = true;
            BitmapImage rightGrayIcon = new BitmapImage();
            rightGrayIcon.BeginInit();
            rightGrayIcon.UriSource = new Uri(@"D:\Studying\Programming\ImageEditor\WPF_GUI\icons\right.ico", UriKind.Absolute);
            rightGrayIcon.EndInit();
            rightIco.Source = rightGrayIcon;

            leftButton.IsEnabled = true;
            BitmapImage leftGrayIcon = new BitmapImage();
            leftGrayIcon.BeginInit();
            leftGrayIcon.UriSource = new Uri(@"D:\Studying\Programming\ImageEditor\WPF_GUI\icons\left.ico", UriKind.Absolute);
            leftGrayIcon.EndInit();
            leftIco.Source = leftGrayIcon;
        }
    }
}
