using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
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
        private ObservableCollection<Filter> filterCollection;

        public FilterViewModel()
        {
            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
            RefreshCommand = new RelayCommand(RefreshFilterTemplates);

            Rows = 3;
            Cols = 3;

            Filter = new Filter(Rows,Cols) {Name = "Custom"};

            RefreshFilterTemplates(null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool DialogResult { get; set; }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public Filter Filter { get; set; }

        public ObservableCollection<Filter> FilterCollection
        {
            get
            {
                return filterCollection;
            }

            set
            {
                filterCollection = value;
                OnPropertyChanged(nameof(FilterCollection));
            }
        }

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

        public void RefreshFilterTemplates(object parameter)
        {
            FilterCollection = new ObservableCollection<Filter>(
                    typeof(StandartFilters).GetProperties().Select(p => p.GetValue(null) as Filter));

            FilterCollection.Add(Filter);

            var comboBox = parameter as ComboBox;
            if (comboBox != null) comboBox.SelectedIndex = 0;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
