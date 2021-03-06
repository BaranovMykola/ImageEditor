﻿namespace WPF_GUI.ViewModel
{
    using System;
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
            ApplyFunctionCommand = new RelayCommand(ApplyFunction, s => !string.IsNullOrEmpty(Function));

            rows = 3;
            cols = 3;

            Filter = new Filter(rows, cols) { Name = "Custom" };
            CurrentFilter = Filter;

            RefreshFilterTemplates(null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool DialogResult { get; set; }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand ApplyFunctionCommand { get; set; }

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
                anchorX = (int)(CurrentFilter?.Anchor.X ?? -1);
                anchorY = (int)(CurrentFilter?.Anchor.Y ?? -1);
                rows = CurrentFilter?.Matrix.Count ?? 0;
                cols = CurrentFilter?.Matrix.FirstOrDefault()?.Count ?? 0;
                RefreshAnchor();
                OnPropertyChanged(nameof(CurrentFilter));
                OnPropertyChanged(nameof(AnchorX));
                OnPropertyChanged(nameof(AnchorY));
                OnPropertyChanged(nameof(Rows));
                OnPropertyChanged(nameof(Cols));
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

        public string Function { get; set; }

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
            CurrentFilter.GenerateMatrix(rows, cols, new NCalc.Expression(string.IsNullOrEmpty(Function) ? "0" : Function));
            var anchor = CurrentFilter.Anchor;
            anchorX = (int)anchor.X;
            anchorY = (int)anchor.Y;
            OnPropertyChanged(nameof(AnchorX));
            OnPropertyChanged(nameof(AnchorY));
        }

        private void RefreshAnchor()
        {
            if (CurrentFilter != null)
            {
                var anchor = CurrentFilter.Anchor;
                if (Math.Abs(anchor.X - (-1)) > float.Epsilon && Math.Abs(anchor.Y - (-1)) > float.Epsilon)
                {
                    CurrentFilter.Matrix[(int)CurrentFilter.Anchor.X][(int)CurrentFilter.Anchor.Y].IsAnchor = false;
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

        private void ApplyFunction(object parameter)
        {
            var func = new NCalc.Expression(Function)
            {
                Parameters =
                {
                    ["anchX"] = (float) AnchorX,
                    ["anchY"] = (float) AnchorY,
                    ["rows"] = (float) Rows,
                    ["cols"] = (float) Cols
                }
            };

            try
            {
                for (int x = 0; x < CurrentFilter.Matrix.Count; x++)
                {
                    for (int y = 0; y < CurrentFilter.Matrix.FirstOrDefault()?.Count; y++)
                    {
                        func.Parameters["y"] = (float)x;
                        func.Parameters["x"] = (float)y;
                        float digit = (float)func.ComputeDouble();
                        CurrentFilter.Matrix[x][y].Coeficient = digit;
                    }
                }

                OnPropertyChanged(nameof(CurrentFilter));
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception.Message}", "Function Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
