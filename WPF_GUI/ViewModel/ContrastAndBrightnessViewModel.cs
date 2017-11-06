namespace WPF_GUI.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using WPF_GUI.Command;
    using WPF_GUI.Properties;

    internal class ContrastAndBrightnessViewModel : INotifyPropertyChanged, IImageDialog
    {
        private double birghtness;

        private float contrast;

        public ContrastAndBrightnessViewModel()
        {
            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double Birghtness
        {
            get
            {
                return birghtness; 
            }

            set
            {
                birghtness = value;
                OnPropertyChanged(nameof(Birghtness));
                Console.WriteLine(birghtness);
            }
        }

        public float Contrast
        {
            get
            {
                return contrast;
            }

            set
            {
                contrast = value;
                OnPropertyChanged(nameof(Contrast));
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
