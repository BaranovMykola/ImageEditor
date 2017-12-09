namespace WPF_GUI.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using CoreWrapper;
    using Microsoft.Win32;
    using WPF_GUI.Const;
    using WPF_GUI.ImageContainer;
    using WPF_GUI.ViewModel.Command;
    using Image = System.Windows.Controls.Image;

    /// <summary>
    /// ViewModel for MainWindow
    /// </summary>
    internal partial class ViewModel : INotifyPropertyChanged
    {
        #region Private Members

        private readonly ImageProc editor = new ImageProc();

        private ObservableCollection<Image> imagesPreview = new ObservableCollection<Image>();

        private ImageSource currentView;

        private int currentIndex;

        private int viewSeletedIndex;

        #endregion

        public ViewModel(WindowMediator contrastMediator, WindowMediator rotateMediator, WindowMediator resizeMediator, WindowMediator filterWindowMediator)
        {
            OpenImageCommand = new RelayCommand(OpenImage, s => IsView);
            OpenedImage = new ImageStorageModel();
            NextCommand = new RelayCommand(s => ++CurrentIndex, s => OpenedImage.IsNext && IsView);
            PrevCommand = new RelayCommand(s => --CurrentIndex, s => OpenedImage.IsPrev && IsView);
            RemoveCommand = new RelayCommand(RemoveImage, s => !OpenedImage.IsEmpty);
            SaveCommand = new RelayCommand(SaveImage, s => IsEdit);
            RotateCommand = new RelayCommand(OpenRotate, s => !OpenedImage.IsEmpty);
            ResizeCommand = new RelayCommand(OpenResize, s => !OpenedImage.IsEmpty);
            ContrastAndBrightnessCommand = new RelayCommand(OpenBrightness, s => !OpenedImage.IsEmpty);
            DetectFaceCommand = new RelayCommand(DetectFace, s => !OpenedImage.IsEmpty);
            PalettingCommand = new RelayCommand(Paletting, s => !OpenedImage.IsEmpty);
            FilterCommand = new RelayCommand(Filter, s => !OpenedImage.IsEmpty);
            GrayscaleCommand = new RelayCommand(Grayscale, s => !OpenedImage.IsEmpty);

            ContrastAndBrightnessWindowContrastMediator = contrastMediator;
            RotateWindowMediator = rotateMediator;
            ResizeWindowMediator = resizeMediator;
            FilterWindowMediator = filterWindowMediator;

            ContrastAndBrightnessWindowContrastMediator.OnClose += BrigthnessWindowClosed;
            RotateWindowMediator.OnClose += RotateClosed;
            ResizeWindowMediator.OnClose += ResizeWindowClosed;
            FilterWindowMediator.OnClose += FilterClosed;

            BrightnessViewModel.PropertyChanged += BrigthnessChanged;
            RotateViewModel.PropertyChanged += RotateChanged;
            OpenedImage.PropertyChanged += UpdateCurrentView;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        public ImageStorageModel OpenedImage { get; set; }

        public ObservableCollection<Image> ImagesPreview
        {
            get
            {
                return imagesPreview;
            }

            set
            {
                imagesPreview = value;
                OnPropertyChanged("ImagesPreview");
            }
        }

        public WindowMediator ContrastAndBrightnessWindowContrastMediator { get; set; }

        public WindowMediator RotateWindowMediator { get; set; }

        public WindowMediator ResizeWindowMediator { get; set; }

        public WindowMediator FilterWindowMediator { get; set; }

        public ContrastAndBrightnessViewModel BrightnessViewModel { get; set; } = new ContrastAndBrightnessViewModel();

        public RotateViewModel RotateViewModel { get; set; } = new RotateViewModel();

        public ResizeViewModel ResizeViewModel { get; set; } = new ResizeViewModel();

        public FilterViewModel FilterViewModel { get; set; } = new FilterViewModel();

        public ImageSource CurrentView
        {
            get
            {
                return currentView;
            }

            set
            {
                currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ProgrammState ViewModelState { get; set; } = ProgrammState.View;

        public bool IsView => ViewModelState == ProgrammState.View;

        public bool IsEdit => ViewModelState == ProgrammState.Edit;

        public bool IsRevert => ViewModelState == ProgrammState.Revert;

        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }

            set
            {
                bool isNewIndex = value != currentIndex && value != -1;
                if (isNewIndex)
                {
                    if (IsView)
                    {
                        currentIndex = value;
                        OpenedImage.CurrentIndex = CurrentIndex;
                    }
                    else if (IsEdit && !IsRevert)
                    {
                        RevertChanges(value);
                        currentIndex = ImagesPreview.Count - 1;
                    }

                    OnPropertyChanged(nameof(CurrentIndex));
                }
            }
        }

        #endregion

        #region Commands Properties

        public RelayCommand OpenImageCommand { get; set; }

        public RelayCommand NextCommand { get; set; }

        public RelayCommand PrevCommand { get; set; }

        public RelayCommand RemoveCommand { get; set; }

        public RelayCommand ContrastAndBrightnessCommand { get; set; }

        public RelayCommand SaveCommand { get; set; }

        public RelayCommand RotateCommand { get; set; }

        public RelayCommand ResizeCommand { get; set; }

        public RelayCommand DetectFaceCommand { get; set; }

        public RelayCommand PalettingCommand { get; set; }

        public RelayCommand FilterCommand { get; set; }

        public RelayCommand GrayscaleCommand { get; set; }

        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Private methods

        private void OpenImage(object parameter)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Multiselect = true;
            openDialog.ShowDialog();
            var pathes = openDialog.FileNames;

            OpenedImage.LoadImages(pathes);
            LoadPreviews(pathes);
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
                ImagesPreview.Add(im);
            }
        }

        private void RemoveImage(object parametr)
        {
            if (IsView)
            {
                RemoveImageFromView();
            }
            else if (IsEdit)
            {
                DiscardChanges();
            }
        }

        private void DiscardChanges()
        {
            MessageBoxResult confirm = MessageBox.Show("Are you sure to discard all changes?", "Discarding all changes...", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.DefaultDesktopOnly);
            if (confirm == MessageBoxResult.Yes)
            {
                ImagesPreview.Clear();
                LoadPreviews(OpenedImage.GetAllPathes());
                ViewModelState = ProgrammState.View;
                CurrentIndex = 0;
                RestoreSelectedIndex();
            }
        }

        private void RemoveImageFromView()
        {
            OpenedImage.Remove();
            ImagesPreview.RemoveAt(CurrentIndex);
            OnPropertyChanged(nameof(OpenedImage));
            Console.WriteLine(OpenedImage.CurrentIndex);
        }

        private void BrigthnessWindowClosed(object sender, EventArgs e)
        {
            if (BrightnessViewModel.DialogResult)
            {
                editor.apply();
                AddPreviewIcon(CurrentView);
            }
            else
            {
                CurrentView = ConvertBitmapToImageSource(editor.getSource());
            }

            SetSelectedLast();
        }

        private void SetSelectedLast()
        {
            CurrentIndex = ImagesPreview.Count - 1;
        }

        private void BrigthnessChanged(object sender, EventArgs e)
        {
            editor.applyContrastAndBrightness(BrightnessViewModel.Contrast, (int)BrightnessViewModel.Birghtness);
            this.CurrentView = ConvertBitmapToImageSource(editor.getPreview());
        }

        private void UpdateCurrentView(object sender, EventArgs e)
        {
            CurrentView = OpenedImage.Current;
        }

        private void SaveImage(object parametr)
        {
            FileDialog save = new SaveFileDialog();
            save.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png|BMP(*.bmp)|*.bmp";
            var confirm = save.ShowDialog();
            if (confirm ?? false)
            {
                string path = save.FileName;
                editor.save(path);
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

        private void AddPreviewIcon(ImageSource icon)
        {
            Image im = new Image()
            {
                Source = icon,
                Height = Constants.PreviewHeight,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            ImagesPreview.Add(im);
        }

        private void RevertChanges(int selectedIndex)
        {
            if (selectedIndex < ImagesPreview.Count - 1)
            {
                var confirm = MessageBox.Show("Are you sure to restore image?", "Restoring...", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.DefaultDesktopOnly) ==
                    MessageBoxResult.Yes;
                if (confirm)
                {
                    ViewModelState = ProgrammState.Revert;
                    editor.restore(selectedIndex);
                    CurrentView = ConvertBitmapToImageSource(editor.getSource());
                    for (int i = ImagesPreview.Count - 1; i > selectedIndex; i--)
                    {
                        ImagesPreview.RemoveAt(i);
                    }

                    ViewModelState = ProgrammState.Edit;
                }
            }
        }

        private void OpenBrightness(object parameter)
        {
            StoreSelectedIndex();
            if (IsView)
            {
                editor.loadImage(OpenedImage.CurrentPath);
                var v = CurrentView;
                ImagesPreview.Clear();
                AddPreviewIcon(v);
            }

            ViewModelState = ProgrammState.Edit;
            BrightnessViewModel.DialogResult = false;
            ContrastAndBrightnessWindowContrastMediator.ShowDialog(BrightnessViewModel);
        }

        private void StoreSelectedIndex()
        {
            viewSeletedIndex = CurrentIndex;
        }

        private void RestoreSelectedIndex()
        {
            CurrentIndex = viewSeletedIndex;
        }

        private void RotateClosed(object sender, EventArgs e)
        {
            if (RotateViewModel.DialogResult)
            {
                editor.apply();
                AddPreviewIcon(CurrentView);
            }
            else
            {
                CurrentView = ConvertBitmapToImageSource(editor.getSource());
            }

            SetSelectedLast();
        }

        private void OpenRotate(object parameter)
        {
            StoreSelectedIndex();
            if (IsView)
            {
                editor.loadImage(OpenedImage.CurrentPath);
                var v = CurrentView;
                ImagesPreview.Clear();
                AddPreviewIcon(v);
            }

            ViewModelState = ProgrammState.Edit;
            RotateViewModel.DialogResult = false;
            RotateWindowMediator.ShowDialog(RotateViewModel);
        }

        private void RotateChanged(object sender, EventArgs e)
        {
            editor.applyRotate((float)RotateViewModel.Angle);
            this.CurrentView = ConvertBitmapToImageSource(editor.getPreview());
        }

        private void OpenResize(object parameter)
        {
            StoreSelectedIndex();
            if (IsView)
            {
                editor.loadImage(OpenedImage.CurrentPath);
                var v = CurrentView;
                ImagesPreview.Clear();
                AddPreviewIcon(v);
            }

            ViewModelState = ProgrammState.Edit;

            ResizeViewModel.Heigth = (int)CurrentView.Height;
            ResizeViewModel.Width = (int)CurrentView.Width;
            ResizeViewModel.ScaleRatio = 1;
            ResizeWindowMediator.ShowDialog(ResizeViewModel);
        }

        private void ResizeWindowClosed(object sender, EventArgs e)
        {
            if (ResizeViewModel.DialogResult)
            {
                try
                {
                    editor.applyResize((float)ResizeViewModel.ScaleRatio);
                    editor.apply();
                    CurrentView = ConvertBitmapToImageSource(editor.getSource());
                    AddPreviewIcon(CurrentView);
                }
                catch
                {
                    MessageBox.Show("You cannot apply this change", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            SetSelectedLast();
        }

        private void DetectFace(object parameter)
        {
            StoreSelectedIndex();
            if (IsView)
            {
                editor.loadImage(OpenedImage.CurrentPath);
                var v = CurrentView;
                ImagesPreview.Clear();
                AddPreviewIcon(v);
            }

            ViewModelState = ProgrammState.Edit;

            editor.detectFace();
            CurrentView = ConvertBitmapToImageSource(editor.getSource());
            AddPreviewIcon(CurrentView);
        }

        private void Paletting(object parameter)
        {
            StoreSelectedIndex();
            if (IsView)
            {
                editor.loadImage(OpenedImage.CurrentPath);
                var v = CurrentView;
                ImagesPreview.Clear();
                AddPreviewIcon(v);
            }

            ViewModelState = ProgrammState.Edit;

            editor.paletting();
            CurrentView = ConvertBitmapToImageSource(editor.getSource());
            AddPreviewIcon(CurrentView);
        }

        private void Filter(object parameter)
        {
            FilterWindowMediator.ShowDialog(FilterViewModel);
        }

        private void Grayscale(object parameter)
        {
            StoreSelectedIndex();
            if (IsView)
            {
                editor.loadImage(OpenedImage.CurrentPath);
                var v = CurrentView;
                ImagesPreview.Clear();
                AddPreviewIcon(v);
            }

            ViewModelState = ProgrammState.Edit;

            editor.toGrayScale();
            CurrentView = ConvertBitmapToImageSource(editor.getSource());
            AddPreviewIcon(CurrentView);
        }

        private void FilterClosed(object sender, EventArgs e)
        {
            StoreSelectedIndex();
            if (IsView)
            {
                editor.loadImage(OpenedImage.CurrentPath);
                var v = CurrentView;
                ImagesPreview.Clear();
                AddPreviewIcon(v);
            }

            ViewModelState = ProgrammState.Edit;

            editor.filter(FilterViewModel.CurrentFilter);
            CurrentView = ConvertBitmapToImageSource(editor.getSource());
            AddPreviewIcon(CurrentView);
        }

        #endregion
    }
}
