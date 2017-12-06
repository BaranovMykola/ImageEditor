namespace WPF_GUI.ViewModel
{
    using System.Windows;
    using System.Windows.Input;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Command;
    using Annotations;
    using FilterEntity;

    internal class FilterViewModel : IImageDialog, INotifyPropertyChanged
    {
        private int rows;

        private int cols;

        private ObservableCollection<Filter> filterCollection;

        private Filter currentFilter;

        private int anchorX;

        private int anchorY;

        public FilterViewModel()
        {
            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
            RefreshCommand = new RelayCommand(RefreshFilterTemplates);

            Rows = 3;
            Cols = 3;

            Filter = new Filter(Rows, Cols) { Name = "Custom" };
            CurrentFilter = Filter;

            RefreshFilterTemplates(null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool DialogResult { get; set; }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public Filter Filter { get; set; }

        public Filter CurrentFilter
        {
            get
            {
                return currentFilter;
            }

            set
            {
                currentFilter = value;
                anchorX = CurrentFilter?.Anchor.X ?? -1;
                anchorY = CurrentFilter?.Anchor.Y ?? -1;
                RefreshAnchor();
                OnPropertyChanged(nameof(CurrentFilter));
                OnPropertyChanged(nameof(AnchorX));
                OnPropertyChanged(nameof(AnchorY));
            }
        }

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

        public int AnchorX
        {
            get
            {
                return anchorX;
            }

            set
            {
                anchorX = value;
                OnPropertyChanged(nameof(AnchorX));
                RefreshAnchor();
            }
        }

        public int AnchorY
        {
            get
            {
                return anchorY;
            }

            set
            {
                anchorY = value;
                OnPropertyChanged(nameof(AnchorY));
                RefreshAnchor();
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
            Close(parameter);
        }

        public void Close(object parameter)
        {
            (parameter as Window)?.Close();
        }

        public void RefreshFilterTemplates(object parameter)
        {
            FilterCollection = new ObservableCollection<Filter>(typeof(StandartFilters).GetProperties().Select(p => p.GetValue(null) as Filter)) { Filter };
            CurrentFilter = Filter;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ResizeFilter()
        {
            CurrentFilter = new Filter(rows, cols) { Name = "Custom" };
            OnPropertyChanged(nameof(Filter));
        }

        private void RefreshAnchor()
        {
            if (CurrentFilter != null)
            {
                var anchor = CurrentFilter.Anchor;
                if (anchor.X != -1 && anchor.Y != -1)
                {
                    CurrentFilter.Matrix[CurrentFilter.Anchor.X][CurrentFilter.Anchor.Y].IsAnchor = false;
                }

                if (AnchorX >= 0 && AnchorY >= 0 && AnchorX < CurrentFilter.Matrix.Count &&
                    AnchorY < CurrentFilter.Matrix[0].Count)
                {
                    CurrentFilter.Matrix[AnchorX][AnchorY].IsAnchor = true;
                }
                else
                {
                    CurrentFilter.SetDefaultAnchor();
                }
            }
        }
    }
}
