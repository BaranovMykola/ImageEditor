using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPF_GUI.Annotations;

namespace WPF_GUI
{
    class ContrastAndBrightnessViewModel : INotifyPropertyChanged
    {
        private double _birghtness;
        private float _contrast;

        public event PropertyChangedEventHandler PropertyChanged;

        public ContrastAndBrightnessViewModel()
        {

        }

        public double Birghtness
        {
            get { return _birghtness; }
            set
            {
                _birghtness = value;
                OnPropertyChanged(nameof(Birghtness));
                Console.WriteLine(_birghtness);
            }
        }

        public float Contrast
        {
            get { return _contrast; }
            set
            {
                _contrast = value;
                OnPropertyChanged(nameof(Contrast));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
