namespace WPF_GUI
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using WPF_GUI.Annotations;
    using WPF_GUI.Command;

    public class RotateViewModel : INotifyPropertyChanged
    {
        private double angle;

        public RotateViewModel()
        {
            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double Angle
        {
            get
            {
                return angle;
            }

            set
            {
                angle = Math.Round(value, 1);
                OnPropertyChanged(nameof(Angle));
            }
        }

        public bool DialogResult { get; set; }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }

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
