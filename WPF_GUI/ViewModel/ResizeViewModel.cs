namespace WPF_GUI.ViewModel
{
    using WPF_GUI.ViewModel.Command;

    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using WPF_GUI.Properties;

    internal class ResizeViewModel : INotifyPropertyChanged, IImageDialog
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
            get
            {
                return scaleRatio;  
            }

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

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
