namespace WPF_GUI.ImageContainer
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Properties;

    public class ImageStorageModel : INotifyPropertyChanged
    {
        private readonly List<Uri> imageSourses = new List<Uri>();

        private int currentIndex;

        public ImageStorageModel()
        {
        }

        public ImageStorageModel(string[] pathes)
        {
            LoadImages(pathes);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }

            set
            {
                currentIndex = value < 0 ? 0 : value;
                if (currentIndex >= imageSourses.Count && !IsEmpty)
                {
                    currentIndex = imageSourses.Count - 1;
                }

                OnPropertyChanged();
                OnPropertyChanged();
            }
        }

        public BitmapImage Current
        {
            get
            {
                if (imageSourses.Count != 0)
                {
                    var b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = imageSourses[CurrentIndex];
                    b.EndInit();
                    return b;
                }

                return new BitmapImage();
            }
        }

        public string CurrentPath => imageSourses[CurrentIndex].AbsolutePath;

        public bool IsNext => CurrentIndex + 1 < imageSourses.Count;

        public bool IsPrev => CurrentIndex > 0;

        public bool IsEmpty => imageSourses.Count == 0;

        public void Next()
        {
            ++CurrentIndex;
            OnPropertyChanged();
        }

        public void Prev()
        {
            --CurrentIndex;
            OnPropertyChanged();
        }

        public void Remove()
        {
            if (imageSourses.Count > 0)
            {
                imageSourses.RemoveAt(CurrentIndex);
            }

            --CurrentIndex;
            OnPropertyChanged();
        }

        public void LoadImages(string[] pathes)
        {
            foreach (var path in pathes)
            {
                imageSourses.Add(new Uri(path, UriKind.Absolute));
            }

            OnPropertyChanged();
        }

        public string[] GetAllPathes()
        {
            string[] pathes = new string[imageSourses.Count];
            for (int i = 0; i < imageSourses.Count; i++)
            {
                pathes[i] = imageSourses[i].AbsolutePath;
            }

            return pathes;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
