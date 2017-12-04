using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_GUI.ImageContainer
{
    public class Filter
    {
        public List<List<double>> Matrix { get; set; }

        public Filter()
        {
            Matrix = new List<List<double>>();
        }

        public Filter(int rows, int cols)
        {
            for (int i = 0; i < rows; i++)
            {
                Matrix.Add( new List<double>(cols));
            }
        }

        public Filter(List<List<double>> matrix)
        {
            Matrix = matrix;
        }
    }
}
