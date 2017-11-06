using System.Windows.Input;
using WPF_GUI.Command;

namespace WPF_GUI
{
    public interface IImageDialog
    {
        bool DialogResult { get; set; }
        ICommand OkCommand { get; set; }
        ICommand CancelCommand { get; set; }
        void Ok(object parameter);
        void Cancel(object parameter);
        void Close(object parameter);
    }
}