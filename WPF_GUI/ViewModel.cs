using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CoreWrapper;
using Microsoft.Win32;
using WPF_GUI.Command;
using WPF_GUI.ImageContainer;

namespace WPF_GUI
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ImageStorageModel OpenedImage { get; set; }

        private ImageProc editor = new ImageProc();

        public ViewModel()
        {
            OpenImageCommand = new RelayCommand(OpenImage);
            OpenedImage = new ImageStorageModel();
            NextCommand = new RelayCommand(s => OpenedImage.Next(), o => OpenedImage.IsNext);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RelayCommand OpenImageCommand { get; set; }

        public RelayCommand NextCommand { get; set; }
        public RelayCommand PrevCommand { get; set; }

        private void OpenImage(object parameter)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Multiselect = true;
            openDialog.ShowDialog();
            var pathes = openDialog.FileNames;

            OpenedImage.LoadImages(pathes);
            NextCommand.RaiseCanExecuteChanged();   
        }
    }
}
