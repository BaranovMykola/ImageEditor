﻿namespace WPF_GUI.ImageContainer
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class Filter : INotifyPropertyChanged
    {
        public Filter(int rows, int cols)
        {
            Matrix = new List<List<FilterItem>>();
            if (rows > 0 && cols > 0)
            {
                for (int i = 0; i < rows; i++)
                {
                    Matrix.Add(new List<FilterItem>());
                    for (int j = 0; j < cols; j++)
                    {
                        Matrix[i].Add(new FilterItem(j));
                    }
                }

                Matrix[rows / 2][cols / 2].IsAnchor = true;
            }
        }

        public Filter(List<List<FilterItem>> matrix)
        {
            Matrix = matrix;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<List<FilterItem>> Matrix { get; }

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

        public override string ToString()
        {
            return Name;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
