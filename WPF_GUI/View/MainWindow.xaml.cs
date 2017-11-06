using WPF_GUI.ViewModel.Command;

namespace WPF_GUI
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using System.Windows.Input;
    using System.Collections.Generic;
    using System.Windows.Media;
    using CoreWrapper;
    using Microsoft.Win32;
    using ImageContainer;
    using Const;
    using Image = System.Windows.Controls.Image;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            DataContext = new ViewModel.ViewModel(new WindowMediator() {CreateWindow = () => new ContrastAndBrightness()}, new WindowMediator() {CreateWindow = () => new RotateWindow()}, new WindowMediator() {CreateWindow = () => new ResizeWindow()});
        }
    }
}
