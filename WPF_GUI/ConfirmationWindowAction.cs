using System;
using System.Windows;

namespace WPF_GUI
{
    internal class WindowMediator
    {
        public Func<Window> CreateWindow;

        public event EventHandler OnClose;

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