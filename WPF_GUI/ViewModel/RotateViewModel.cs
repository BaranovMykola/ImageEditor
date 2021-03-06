﻿namespace WPF_GUI.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using Properties;
    using Command;

    internal class RotateViewModel : INotifyPropertyChanged, IImageDialog
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
                angle = Math.Round(value);
                OnPropertyChanged();
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
