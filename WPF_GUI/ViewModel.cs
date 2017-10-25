using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CoreWrapper;
using Microsoft.Win32;
using WPF_GUI.Command;
using WPF_GUI.Const;
using WPF_GUI.ImageContainer;
using Image = System.Windows.Controls.Image;

namespace WPF_GUI
{
    internal partial class ViewModel : INotifyPropertyChanged
    {
        #region Private Members

        private ImageProc editor = new ImageProc();

        private ObservableCollection<Image> _imagesPreview = new ObservableCollection<Image>();

        private ImageSource _currentView;
        private int _currentIndex;

        #endregion

        public ViewModel(WindowMediator mediator)
        {
            OpenImageCommand = new RelayCommand(OpenImage);
            OpenedImage = new ImageStorageModel();
            NextCommand = new RelayCommand(s =>
            {
                //OpenedImage.Next();
                ++CurrentIndex;
            }, s => OpenedImage.IsNext && IsView);
            PrevCommand = new RelayCommand(s =>
            {
                //OpenedImage.Prev();
                --CurrentIndex;
            }, s => OpenedImage.IsPrev && IsView);
            RemoveCommand = new RelayCommand(RemoveImage, s => !OpenedImage.IsEmpty);
            SaveCommand = new RelayCommand(SaveImage, s => IsEdit);
            ContrastAndBrightnessWindowMediator = mediator;
            ContrastAndBrightnessWindowMediator.OnClose += BrigthnessWindowClosed;
            BrightnessViewModel.PropertyChanged += BrigthnessChanged;
            OpenedImage.PropertyChanged += UpdateCurrentView;
            ContrastAndBrightnessCommand = new RelayCommand(s =>
            {
                if (IsView)
                {
                    editor.loadImage(OpenedImage.CurrentPath);
                    var v = CurrentView;
                    ImagesPreview.Clear();
                    AddPreviewIcon(v);
                }
                ViewModelState = ProgrammState.Edit;
                ContrastAndBrightnessWindowMediator.ShowDialog(BrightnessViewModel);
            }, s => !OpenedImage.IsEmpty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        public ImageStorageModel OpenedImage { get; set; }

        public ObservableCollection<Image> ImagesPreview
        {
            get
            {
                return _imagesPreview;
                OnPropertyChanged("ImagesPreview");
            }
            set
            {
                _imagesPreview = value;
                OnPropertyChanged("ImagesPreview");
            }
        }

        public WindowMediator ContrastAndBrightnessWindowMediator { get; set; }

        public ContrastAndBrightnessViewModel BrightnessViewModel { get; set; } = new ContrastAndBrightnessViewModel();

        public ImageSource CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ProgrammState ViewModelState { get; set; } = ProgrammState.View;

        public bool IsView => ViewModelState == ProgrammState.View;

        public bool IsEdit => ViewModelState == ProgrammState.Edit;

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set
            {
                bool u = value != _currentIndex;
                
                _currentIndex = value;
                if (u)
                {
                    if (ViewModelState == ProgrammState.View)
                    {
                        OpenedImage.CurrentIndex = CurrentIndex;
                        OnPropertyChanged(nameof(CurrentIndex));
                    }
                    else if (ViewModelState == ProgrammState.Edit)
                    {
                        Console.WriteLine("index = " + value);
                        RevertChanges(value);
                        OnPropertyChanged(nameof(CurrentIndex));
                    }
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

                double ratio = Constants.PreviewWidth/original.Width;
                var reducedPreview = new BitmapImage();
                reducedPreview.BeginInit();
                reducedPreview.UriSource = new Uri(path);
                reducedPreview.DecodePixelWidth = Constants.PreviewWidth;
                reducedPreview.DecodePixelHeight = (int) (original.Height*ratio);
                reducedPreview.EndInit();

                var im = new Image
                {
                    Source = reducedPreview,
                    Height = Constants.PreviewHeight,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                //preview.Items.Add(im);
                ImagesPreview.Add(im);
            }
        }

        private void RemoveImage(object parametr)
        {
            int pos = OpenedImage.CurrentIndex;
            OpenedImage.Remove();
            ImagesPreview.RemoveAt(OpenedImage.CurrentIndex);
            OpenedImage.CurrentIndex = pos - 1;
            OnPropertyChanged(nameof(OpenedImage));
            Console.WriteLine(OpenedImage.CurrentIndex);
        }

        private void BrigthnessWindowClosed(object sender, EventArgs e)
        {
            if ((sender as Window)?.DialogResult ?? false)
            {
                editor.apply();
                AddPreviewIcon(CurrentView);
            }
            else
            {
                CurrentView = ConvertBitmapToImageSource(editor.getSource());
            }
        }

        private void BrigthnessChanged(object sender, EventArgs e)
        {
            editor.applyContrastAndBrightness(BrightnessViewModel.Contrast, (int) BrightnessViewModel.Birghtness);
            Console.WriteLine("contrast = " + BrightnessViewModel.Contrast);
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
            if (save.ShowDialog() ?? false)
            {
                string path = save.FileName;
                editor.save(path);
                ViewModelState = ProgrammState.View;
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
            MessageBoxResult confirm = MessageBox.Show("Are you sure to restore image?", "Restoring...", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.DefaultDesktopOnly);
            if (confirm == MessageBoxResult.Yes)
            {
                editor.restore(selectedIndex);
                CurrentView = ConvertBitmapToImageSource(editor.getSource());
                var t = ImagesPreview.ToList();
                for (int i = ImagesPreview.Count - 1; i > selectedIndex; i--)
                {
                    ImagesPreview.RemoveAt(i);
                }
                //ImagesPreview = new ObservableCollection<Image>(t);
            }
        }

        #endregion
    }
}
