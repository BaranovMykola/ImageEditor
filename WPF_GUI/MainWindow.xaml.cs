﻿using System.Windows.Media;

namespace WPF_GUI
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using System.Windows.Input;
    using Microsoft.Win32;
    using ImageContainer;
    using Const;
    using Image = System.Windows.Controls.Image;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageStorage openedImage = new ImageStorage();
        private CoreWrapper.ImageProc a;

        public MainWindow()
        {
            this.InitializeComponent();
            InitializeButtonsIcons();
            openedImage.LockLeft += () => LockImageControl(leftButton, leftIco, Icons.left_gray);
            openedImage.LockRight += () => LockImageControl(rightButton, rightIco, Icons.right_gray);
            openedImage.LockRemove += () =>
            {
                LockImageControl(removeButton, removeIco, Icons.remove_gray);
                LockImageControl(editButton, editIco, Icons.edit_gray);
            };
            openedImage.UnlockAll += () =>
            {
                LockImageControl(leftButton, leftIco, Icons.left, true);
                LockImageControl(rightButton, rightIco, Icons.right, true);
                LockImageControl(removeButton, removeIco, Icons.remove, true);
                LockImageControl(editButton, editIco, Icons.edit, true);
            };
            openedImage.ImageChanged += index => preview.SelectedIndex = index;
            a = new CoreWrapper.ImageProc(@"D:\Studying\Programming\ImageEditor\TestImages\ss.jpg", 1000, 1000);
            //var b = a.readOriginalWrapper(@"D:\Studying\Programming\ImageEditor\TestImages\ss.jpg");
        }

        private void InitializeButtonsIcons()
        {
            leftIco.Source = Icons.left_gray.ToImageSource();
            rightIco.Source = Icons.right_gray.ToImageSource();
            removeIco.Source = Icons.remove_gray.ToImageSource();
            editIco.Source = Icons.edit_gray.ToImageSource();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Multiselect = true;
            openDialog.ShowDialog();
            var pathes = openDialog.FileNames;

            openedImage.LoadImages(pathes);
            LoadPreviews(pathes);
            image.Source = openedImage.Current;
            preview.SelectedIndex = openedImage.CurrentIndex;
            preview.Focus();
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
            int index = openedImage.CurrentIndex;
            preview.Items.RemoveAt(index);
            openedImage.Remove();
            image.Source = openedImage.Current;
        }

        private void LoadPreviews(string[] pathes)
        {
            foreach (var path in pathes)
            {
                var original = new BitmapImage();
                original.BeginInit();
                original.UriSource = new Uri(path);
                original.EndInit();

                double ratio = Constants.PreviewWidth / original.Width;
                var reducedPreview = new BitmapImage();
                reducedPreview.BeginInit();
                reducedPreview.UriSource = new Uri(path);
                reducedPreview.DecodePixelWidth = Constants.PreviewWidth;
                reducedPreview.DecodePixelHeight = (int)(original.Height * ratio);
                reducedPreview.EndInit();

                var im = new Image
                {
                    Source = reducedPreview,
                    Height = Constants.PreviewHeight,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                preview.Items.Add(im);
            }
        }

        private void Preview_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (preview.SelectedIndex != -1)
            {
                openedImage.CurrentIndex = preview.SelectedIndex;
                image.Source = openedImage.Current;
            }
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                if (leftButton.IsEnabled)
                {
                    LeftButton_OnClick(this, null);
                }
            }
            else if (e.Key == Key.Right)
            {
                if (rightButton.IsEnabled)
                {
                    RightButton_OnClick(this, null);
                }
            }
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var b = a.readOriginalWrapper(@"D:\Studying\Programming\ImageEditor\TestImages\ss.jpg");
            image.Source = ConvertBitmapToImageSource(b.Clone() as Bitmap);
        }

        private ImageSource ConvertBitmapToImageSource(Bitmap imToConvert)
        {
            Bitmap bmp = new Bitmap(imToConvert);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            BitmapImage image1 = new BitmapImage();
            image1.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image1.StreamSource = ms;
            image1.EndInit();

            ImageSource sc = (ImageSource)image1;

            return sc;
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            a.rotateImage((float)slider.Value);
            var b = a.getPreview(1000, 1000);
            image.Source = ConvertBitmapToImageSource(b);
        }
    }
}
