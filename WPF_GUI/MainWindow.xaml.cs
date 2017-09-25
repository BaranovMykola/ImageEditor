﻿namespace WPF_GUI
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using Microsoft.Win32;
    using WPF_GUI.ImageContainer;
    using Image = System.Windows.Controls.Image;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageStorage openedImage = new ImageStorage();

        public MainWindow()
        {
            this.InitializeComponent();
            InitializeButtonsIcons();
            openedImage.LockLeft = () => LockImageControl(leftButton, leftIco, Icons.left_gray);
            openedImage.LockRight = () => LockImageControl(rightButton, rightIco, Icons.right_gray);
            openedImage.LockRemove = () => LockImageControl(removeButton, removeIco, Icons.remove_gray);
            openedImage.UnlockAll = () =>
            {
                LockImageControl(leftButton, leftIco, Icons.left, true);
                LockImageControl(rightButton, rightIco, Icons.right, true);
                LockImageControl(removeButton, removeIco, Icons.remove, true);
            };
        }

        private void InitializeButtonsIcons()
        {
            leftIco.Source = Icons.left_gray.ToImageSource();
            rightIco.Source = Icons.right_gray.ToImageSource();
            removeIco.Source = Icons.remove_gray.ToImageSource();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
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

        private void LockImageControl(Button button, Image icon, Icon newPicture, bool enabled = false)
        {
            button.IsEnabled = enabled;
            icon.Source = newPicture.ToImageSource();
        }
        
        private void Remove_OnClick(object sender, RoutedEventArgs e)
        {
            openedImage.Remove();
            image.Source = openedImage.Current;
        }
    }
}
