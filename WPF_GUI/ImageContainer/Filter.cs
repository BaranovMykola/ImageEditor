using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPF_GUI.Annotations;

namespace WPF_GUI.ImageContainer
{
    public class Filter
    {
        public List<List<MyInt>> Matrix { get; set; }

        public Filter()
        {
            Matrix = new List<List<MyInt>>();
        }

        public Filter(int rows, int cols): this()
        {
            for (int i = 0; i < rows; i++)
            {
                Matrix.Add((new List<MyInt>()));
                for (int j = 0; j < cols; j++)
                {
                    Matrix[i].Add(new MyInt(j));
                }
            }
        }

        public Filter(List<List<MyInt>> matrix)
        {
            Matrix = matrix;
        }
    }

    public class MyInt : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MyInt()
        {
            
        }

        public MyInt(int myVar)
        {
            this.myVar = myVar;
        }

        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set
            {
                myVar = value;
                OnPropertyChanged(nameof(MyProperty));
            }
        }

        public override string ToString()
        {
            return MyProperty.ToString();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
