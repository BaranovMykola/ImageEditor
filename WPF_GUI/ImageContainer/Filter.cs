using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_GUI.ImageContainer
{
    public class Filter
    {
        public List<List<FilterItem>> Matrix { get; set; }

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
    }
}
