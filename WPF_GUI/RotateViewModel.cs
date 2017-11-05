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
    public class RotateViewModel : INotifyPropertyChanged
    {
        private double _angle;

        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = Math.Round(value, 1);
                OnPropertyChanged(nameof(Angle));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
