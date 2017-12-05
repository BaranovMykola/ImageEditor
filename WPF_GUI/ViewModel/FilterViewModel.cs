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

            Filter = new Filter(Rows,Cols);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool DialogResult { get; set; }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public Filter Filter { get; set; }

        public DataTable dt
        {
            get
            {
                var t = new DataTable();
                for (int i = 0; i < Filter.Matrix.Count; i++)
                {
                    t.Columns.Add(new DataColumn(i.ToString()));
                }

                for (var r = 0; r < rows; r++)
                {
                    var newRow = t.NewRow();
                    for (var c = 0; c < cols; c++)
                    {
                        var v = Filter.Matrix[r][c];

                        // Round if parameter is set
                        newRow[c] = v;
                    }

                    t.Rows.Add(newRow);
                }
                return t;
            }
            set
            {
                var v = value;
                Console.WriteLine("setted");
            }
        }

        public ObservableCollection<row> UserSet { get; set; } = new ObservableCollection<row>() {new row(1,2), new row(3,4)};

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
            Close(parameter);
        }

        public void Close(object parameter)
        {
            (parameter as Window)?.Close();
                                                //w.dataGrid.Items.Refresh();
            //var bindingExpression = BindingOperations.GetBindingExpression(w.dataGrid as DependencyObject, UIElement.RenderTransformProperty);
            //bindingExpression?.UpdateSource();
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


    class row : INotifyPropertyChanged
    {
        private int _a;
        private int _b;

        public int a
        {
            get { return _a; }
            set
            {
                _a = value; 
                OnPropertyChanged(nameof(a));
            }
        }

        public int b
        {
            get { return _b; }
            set
            {
                _b = value; 
                OnPropertyChanged(nameof(b));
            }
        }

        public row(int _a, int _b)
        {
            a = _a;
            b = _b;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
