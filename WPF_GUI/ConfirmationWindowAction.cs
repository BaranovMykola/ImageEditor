using System;
using System.Windows;

namespace WPF_GUI
{
    internal class WindowMediator
    {
        public Func<object, Window> CreateWindow;

        public event EventHandler OnClose;

        public void Show(object parameter)
        {
            var win = CreateWindow.Invoke(parameter);
            win.Closed += OnClose;
            win.Show();
        }

        public void ShowDialog(object parameter)
        {
            var win = CreateWindow.Invoke(parameter);
            win.Closed += OnClose;
            win.ShowDialog();
        }
    }
}