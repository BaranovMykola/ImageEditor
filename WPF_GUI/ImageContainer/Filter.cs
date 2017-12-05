using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_GUI.ImageContainer
{
    public class Filter
    {
        private Point anchor;

        public Filter()
        {
            Matrix = new List<List<FilterItem>>();
        }

        public Filter(int rows, int cols): this()
        {
            for (int i = 0; i < rows; i++)
            {
                Matrix.Add((new List<FilterItem>()));
                for (int j = 0; j < cols; j++)
                {
                    Matrix[i].Add(new FilterItem(j));
                }
            }
        }

        public Filter(List<List<FilterItem>> matrix)
        {
            Matrix = matrix;
        }

        public List<List<FilterItem>> Matrix { get; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
