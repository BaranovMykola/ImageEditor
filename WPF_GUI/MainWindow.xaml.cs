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
        private readonly ImageStorage openedImage = new ImageStorage();
        private ImageProc editor = new ImageProc();

        public MainWindow()
        {
            this.InitializeComponent();
            InitializeButtonsIcons();
            //openedImage.LockLeft += () => LockImageControl(leftButton, leftIco, Icons.left_gray);
            openedImage.UnlockAll += UnlockAllButton;
            openedImage.ImageChanged += index => preview.SelectedIndex = index;
            DataContext = new ViewModel();
        }

        private void InitializeButtonsIcons()
        {
        }

        private void UnlockAllButton()
        {
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

        private ImageSource ConvertBitmapToImageSource(Bitmap bitmapImage)
        {
            Bitmap bmp = new Bitmap(bitmapImage);
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

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            contAndBrightButton.IsEnabled = true;
            rotateButton.IsEnabled = true;
            resizeButton.IsEnabled = true;

            var file = this.openedImage.CurrentPath;
            editor.loadImage(file);

            preview.SelectionChanged -= Preview_OnSelectionChanged;
            preview.MouseDoubleClick += Preview_OnMouseDoubleClick;
            removeButton.Click -= Remove_OnClick;
            removeButton.Click += ReturnToImageviewMode;

            preview.Items.Clear();
            AddPreviewIcon(image);
        }

        private void ReturnToImageviewMode(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirm = MessageBox.Show("Are you sure to discard all changes?", "Discarding all changes...", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.DefaultDesktopOnly);
            if (confirm == MessageBoxResult.Yes)
            {
                preview.Items.Clear();
                LoadPreviews(openedImage.GetAllPathes());
                image.Source = openedImage.Current;

                preview.SelectionChanged += Preview_OnSelectionChanged;
                preview.MouseDoubleClick -= Preview_OnMouseDoubleClick;
                removeButton.Click += Remove_OnClick;
                removeButton.Click -= ReturnToImageviewMode;

                UnlockAllButton();
                LockImageControl(saveButton, saveIco, Icons.save_gray);

                contAndBrightButton.IsEnabled = false;
                rotateButton.IsEnabled = false;
                resizeButton.IsEnabled = false;
            }
        }

        private void AddPreviewIcon(Image icon)
        {
            Image im = new Image()
            {
                Source = icon.Source.Clone(),
                Height = Constants.PreviewHeight,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            preview.Items.Add(im);
        }

        private void ContAndBrightButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new ContrastAndBrightness();
            dialog.PreviewChanged += ChangeContrastAndBrightness;

            var apply = dialog.ShowDialog();
            if (apply != null && apply.Value)
            {
                editor.apply();
                AddPreviewIcon(image);
            }
            else
            {
                image.Source = ConvertBitmapToImageSource(editor.getSource());
            }
        }

        private void LoadPreviewIcons(List<Bitmap> icons)
        {
            preview.Items.Clear();
            foreach (var i in icons)
            {
                var im = new Image();
                im.Source = ConvertBitmapToImageSource(i).Clone();
                image.Source = im.Source.Clone();
                preview.Items.Add(im);
            }
        }

        private void ChangeContrastAndBrightness(float contrast, int brightness)
        {
            editor.applyContrastAndBrightness(contrast, brightness);
            image.Source = ConvertBitmapToImageSource(editor.getPreview());
        }

        private void Preview_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = preview.SelectedIndex;
            if (selectedIndex != -1 && selectedIndex + 1 < preview.Items.Count)
            {
                MessageBoxResult confirm = MessageBox.Show("Are you sure to restore image?", "Restoring...", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.DefaultDesktopOnly);
                if (confirm == MessageBoxResult.Yes)
                {
                    editor.restore(preview.SelectedIndex);
                    image.Source = ConvertBitmapToImageSource(editor.getSource());
                    for (int i = preview.Items.Count - 1; i > selectedIndex; i--)
                    {
                        preview.Items.RemoveAt(i);
                    }
                }
            }
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            FileDialog save = new SaveFileDialog();
            save.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png|BMP(*.bmp)|*.bmp";
            save.ShowDialog();
            string path = save.FileName;
            editor.save(path);
        }

        private void OpenButton_OnClick(object sender, RoutedEventArgs e)
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
    }
}
