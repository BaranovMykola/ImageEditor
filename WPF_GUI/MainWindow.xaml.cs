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

        public MainWindow()
        {
            this.InitializeComponent();
            InitializeButtonsIcons();
            openedImage.LockLeft += () => LockImageControl(leftButton, leftIco, Icons.left_gray);
            openedImage.LockRight += () => LockImageControl(rightButton, rightIco, Icons.right_gray);
            openedImage.LockRemove += () => LockImageControl(removeButton, removeIco, Icons.remove_gray);
            openedImage.UnlockAll += () =>
            {
                LockImageControl(leftButton, leftIco, Icons.left, true);
                LockImageControl(rightButton, rightIco, Icons.right, true);
                LockImageControl(removeButton, removeIco, Icons.remove, true);
            };
            openedImage.ImageChanged += index => preview.SelectedIndex = index;
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
            openedImage.CurrentIndex = preview.SelectedIndex;
            image.Source = openedImage.Current;
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
    }
}
