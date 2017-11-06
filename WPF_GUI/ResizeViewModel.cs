using System;
using System.CodeDom;
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
        private double scaleRatio = 1;

        public ResizeViewModel()
        {
            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Width { get; set; }

        public int Heigth { get; set; }

        public double ScaleRatio
        {
            get { return scaleRatio; }
            set
            {
                if (value <= 0)
                {
                    value = 1;
                }
                scaleRatio = Math.Round(value, 2); 
                OnPropertyChanged(nameof(ScaleRatio));
            }
        }

        public bool DialogResult { get; set; }

        public RelayCommand OkCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

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
