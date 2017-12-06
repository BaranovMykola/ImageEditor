using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPF_GUI.Annotations;

namespace WPF_GUI.ImageContainer
{
    public class Filter : INotifyPropertyChanged
    {
        public Filter(int rows, int cols)
        {
            Matrix = new List<List<FilterItem>>();
            if (rows > 0 && cols > 0)
            {
                for (int i = 0; i < rows; i++)
                {
                    Matrix.Add((new List<FilterItem>()));
                    for (int j = 0; j < cols; j++)
                    {
                        Matrix[i].Add(new FilterItem(j));
                    }
                }

                Matrix[rows/2][cols/2].IsAnchor = true;
            }
        }

        public Filter(List<List<FilterItem>> matrix)
        {
            Matrix = matrix;
        }

        public List<List<FilterItem>> Matrix { get; }

        public string Name { get; set; }

        public Point Anchor
        {
            get
            {
                for (int i = 0; i < Matrix[0].Count; i++)
                {
                    for (int j = 0; j < Matrix.Count; j++)
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

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
