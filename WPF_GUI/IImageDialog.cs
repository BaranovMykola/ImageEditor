using WPF_GUI.Command;

namespace WPF_GUI
{
    public interface IImageDialog
    {
        bool DialogResult { get; set; }
        RelayCommand OkCommand { get; set; }
        RelayCommand CancelCommand { get; set; }
        void Ok(object parameter);
        void Cancel(object parameter);
        void Close(object parameter);
    }
}