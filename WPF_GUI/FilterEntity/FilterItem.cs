namespace WPF_GUI.FilterEntity
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class FilterItem : INotifyPropertyChanged
    {
        private float coeficient;

        private bool isAnchor;

        public FilterItem()
        {
        }

        public FilterItem(float coeficient)
        {
            this.coeficient = coeficient;
        }

        public FilterItem(float coeficient, bool isAnchor)
        {
            Coeficient = coeficient;
            IsAnchor = isAnchor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public float Coeficient
        {
            get
            {
                return coeficient;
            }

            set
            {
                coeficient = value;
                OnPropertyChanged(nameof(Coeficient));
            }
        }

        public bool IsAnchor
        {
            get
            {
                return isAnchor;
            }

            set
            {
                isAnchor = value;
                OnPropertyChanged(nameof(IsAnchor));
            }
        }

        public override string ToString() => Coeficient.ToString(CultureInfo.InvariantCulture);

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}