using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_GUI.ViewModel.Command;

namespace WPF_GUI.ViewModel
{
    internal class FilterViewModel : IImageDialog
    {
        public FilterViewModel()
        {
            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
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
    }
}
