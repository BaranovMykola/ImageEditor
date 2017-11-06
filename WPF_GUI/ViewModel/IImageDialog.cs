namespace WPF_GUI
{
    using System.Windows.Input;

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