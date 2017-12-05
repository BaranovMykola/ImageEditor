using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using WPF_GUI.Annotations;

namespace WPF_GUI.ImageContainer
{
    public class FilterItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public FilterItem()
        {
            
        }

        public FilterItem(float coeficient)
        {
            this.coeficient = coeficient;
        }

        private float coeficient;

        public float Coeficient
        {
            get { return coeficient; }
            set
            {
                coeficient = value;
                OnPropertyChanged(nameof(Coeficient));
            }
        }

        public override string ToString()
        {
            return Coeficient.ToString(CultureInfo.InvariantCulture);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}