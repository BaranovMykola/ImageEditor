namespace WPF_GUI
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using WPF_GUI.Annotations;

    internal class ContrastAndBrightnessViewModel : INotifyPropertyChanged
    {
        private double birghtness;

        private float contrast;

        public event PropertyChangedEventHandler PropertyChanged;

        public double Birghtness
        {
            get
            {
                return birghtness; 
            }

            set
            {
                birghtness = value;
                OnPropertyChanged(nameof(Birghtness));
                Console.WriteLine(birghtness);
            }
        }

        public float Contrast
        {
            get
            {
                return contrast;
            }

            set
            {
                contrast = value;
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
