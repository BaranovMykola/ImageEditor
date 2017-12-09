namespace FilterEntity
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;

    public class Filter : INotifyPropertyChanged
    {
        public Filter(int rows, int cols)
        {
            GenerateMatrix(rows, cols, new NCalc.Expression("0"));
        }

        public Filter(int rows, int cols, NCalc.Expression generator) : this(rows, cols)
        {
            GenerateMatrix(rows, cols, generator);
        }

        public Filter(ObservableCollection<ObservableCollection<FilterItem>> matrix)
        {
            Matrix = matrix;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ObservableCollection<FilterItem>> Matrix { get; private set; }

        public string Name { get; set; }

        public Point Anchor
        {
            get
            {
                for (int i = 0; i < Matrix.Count; i++)
                {
                    for (int j = 0; j < Matrix.FirstOrDefault()?.Count; j++)
                    {
                        if (Matrix[i][j].IsAnchor)
                        {
                            return new Point(i, j);
                        }
                    }
                }

                return new Point(-1, -1);
            }
        }

        public void GenerateMatrix(int rows, int cols, NCalc.Expression generator)
        {
            Matrix = new ObservableCollection<ObservableCollection<FilterItem>>();

            if (rows > 0 && cols > 0)
            {
                try
                {
                    for (int i = 0; i < rows; i++)
                    {
                        Matrix.Add(new ObservableCollection<FilterItem>());
                        for (int j = 0; j < cols; j++)
                        {
                            generator.Parameters["y"] = (float)i;
                            generator.Parameters["x"] = (float)j;
                            Matrix[i].Add(new FilterItem((float)generator.ComputeDouble()));
                        }
                    }

                    Matrix[rows / 2][cols / 2].IsAnchor = true;
                    OnPropertyChanged(nameof(Matrix));
                    OnPropertyChanged(nameof(Anchor));
                }
                catch (Exception)
                {
                    GenerateMatrix(rows, cols, new NCalc.Expression("0"));
                }
            }
        }

        public void SetDefaultAnchor()
        {
            foreach (var row in Matrix)
            {
                foreach (var filterItem in row)
                {
                    filterItem.IsAnchor = false;
                }
            }

            var rows = Matrix.Count;
            if (rows > 0)
            {
                var cols = Matrix.FirstOrDefault()?.Count ?? 0;
                Matrix[rows / 2][cols / 2].IsAnchor = true;
            }
        }

        public override string ToString() => Name;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
