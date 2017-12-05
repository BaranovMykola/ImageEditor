using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using WPF_GUI.Annotations;
using WPF_GUI.View;

namespace WPF_GUI.ViewModel
{
    using System.Windows;
    using System.Windows.Input;
    using ImageContainer;
    using Command;

    internal class FilterViewModel : IImageDialog, INotifyPropertyChanged
    {
        private int rows;
        private int cols;

        public FilterViewModel()
        {
            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);

            Rows = 3;
            Cols = 3;

            Filter = new Filter(Rows,Cols) {Name = "Custom"};

            var box = new Filter(5,5) {Name = "Box Filter"};
            foreach (var i in box.Matrix)
            {
                foreach (var j in i)
                {
                    j.Coeficient = (float) (1.0/25);
                }
            }

            var custom = Filter;

            FilterCollection = new ObservableCollection<Filter>() {box,custom};
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool DialogResult { get; set; }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public Filter Filter { get; set; }

        public ObservableCollection<Filter> FilterCollection { get; set; }

        public int Rows
        {
            get
            {
                return rows;
            }

            set
            {
                rows = value;
                OnPropertyChanged(nameof(Rows));
                ResizeFilter();
            }
        }

        public int Cols
        {
            get
            {
                return cols;
            }

            set
            {
                cols = value;
                OnPropertyChanged(nameof(Cols));
                ResizeFilter();
            }
        }

        public void Ok(object parameter)
        {
            DialogResult = true;
            Close(parameter);
        }

        public void Cancel(object parameter)
        {
            DialogResult = false;
            //Close(parameter);
        }

        public void Close(object parameter)
        {
            (parameter as Window)?.Close();
        }

        private void ResizeFilter()
        {
            Filter = new Filter(rows,cols);
            OnPropertyChanged(nameof(Filter));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
