namespace WPF_GUI.ViewModel.Command
{
    using System;
    using System.Windows;

    internal class WindowMediator
    {
        public event EventHandler OnClose;

        public Func<Window> CreateWindow { get; set; }

        public void Show(object dataContext)
        {
            var win = CreateWindow.Invoke();
            win.Closed += OnClose;
            win.DataContext = dataContext;
            win.Show();
        }

        public void ShowDialog(object dataContext)
        {
            var win = CreateWindow.Invoke();
            win.DataContext = dataContext;
            win.Closed += OnClose;
            win.ShowDialog();
        }
    }
}