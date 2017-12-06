namespace WPF_GUI.View
{
    using System.Windows;
    using ViewModel.Command;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var contrastWindow = new WindowMediator { CreateWindow = () => new ContrastAndBrightness() };
            var rotateWindow = new WindowMediator { CreateWindow = () => new RotateWindow() };
            var resizeWindow = new WindowMediator { CreateWindow = () => new ResizeWindow() };
            var filterWindow = new WindowMediator { CreateWindow = () => new FilterWindow() };
            DataContext = new ViewModel.ViewModel(contrastWindow, rotateWindow, resizeWindow, filterWindow);
        }
    }
}
