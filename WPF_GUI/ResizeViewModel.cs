using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_GUI.Annotations;
using WPF_GUI.Command;

namespace WPF_GUI
{
    public class ResizeViewModel: INotifyPropertyChanged, IImageDialog
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Width { get; set; }

        public int Heigth { get; set; }

        public double ScaleRatio { get; set; }

        public bool DialogResult { get; set; }

        public RelayCommand OkCommand { get; set; }

        public RelayCommand CancelDialog { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Ok(object parameter)
        {
            DialogResult = true;
            Close(parameter);
        }

        public void Cancel(object parameter)
        {
            DialogResult = false;
            Close(parameter);
        }

        public void Close(object parameter)
        {
            (parameter as Window)?.Close();
        }
    }
}
