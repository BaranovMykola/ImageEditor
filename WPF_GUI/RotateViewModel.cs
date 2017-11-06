using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_GUI.Annotations;
using WPF_GUI.Command;

namespace WPF_GUI
{
    public class RotateViewModel : INotifyPropertyChanged
    {
        private double _angle;

        public RotateViewModel()
        {
            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
        }

        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = Math.Round(value, 1);
                OnPropertyChanged(nameof(Angle));
            }
        }

        public bool DialogResult { get; set; }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Ok(object parameter)
        {
            DialogResult = true;
            Close(parameter);
        }

        private void Cancel(object parameter)
        {
            DialogResult = false;
            Close(parameter);
        }

        private void Close(object parameter)
        {
            (parameter as Window)?.Close();
        }
    }
}
