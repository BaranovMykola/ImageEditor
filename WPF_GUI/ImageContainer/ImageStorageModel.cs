using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPF_GUI.Annotations;

namespace WPF_GUI.ImageContainer
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;

    public class ImageStorageModel : INotifyPropertyChanged
    {
        private List<Uri> imageSourses = new List<Uri>();

        public ImageStorageModel()
        {
        }

        public ImageStorageModel(string[] pathes)
        {
            LoadImages(pathes);
        }

        public event Action LockLeft;

        public event Action LockRight;

        public event Action LockRemove;

        public event Action UnlockAll;

        public event Action<int> ImageChanged;

        public int CurrentIndex { get; set; } = 0;

        public BitmapImage Current
        {
            get
            {
                UnlockAll?.Invoke();
                if (CurrentIndex == 0)
                {
                    LockLeft?.Invoke();
                }

                if (CurrentIndex + 1 == imageSourses.Count)
                {
                    LockRight?.Invoke();
                }

                if (imageSourses.Count != 0)
                {
                    var b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = imageSourses[CurrentIndex];
                    b.EndInit();
                    return b;
                }
                else
                {
                    LockLeft?.Invoke();
                    LockRight?.Invoke();
                    LockRemove?.Invoke();
                    return new BitmapImage();
                }
            }
        }

        public string CurrentPath
        {
            get { return imageSourses[CurrentIndex].AbsolutePath; }
        }

        public void Next()
        {
            ++CurrentIndex;
            ImageChanged?.Invoke(CurrentIndex);
            OnPropertyChanged(nameof(Current));
        }

        public void Prev()
        {
            --CurrentIndex;
            ImageChanged?.Invoke(CurrentIndex);
            OnPropertyChanged(nameof(Current));
        }

        public void Remove()
        {
            if (imageSourses.Count > 0)
            {
                if (imageSourses.Count == 1)
                {
                    LockRemove?.Invoke();
                }

                imageSourses.RemoveAt(CurrentIndex);
                if (imageSourses.Count > 0)
                {
                    if (CurrentIndex == imageSourses.Count)
                    {
                        --CurrentIndex;
                    }

                    if (CurrentIndex == -1)
                    {
                        ++CurrentIndex;
                    }
                }
            }
        }

        public void LoadImages(string[] pathes)
        {
            foreach (var path in pathes)
            {
                imageSourses.Add(new Uri(path, UriKind.Absolute));
            }
            OnPropertyChanged(nameof(Current));
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

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsNext {
            get
            {
                return CurrentIndex+1 < imageSourses.Count;
            }
        }

        public bool IsPrev
        {
            get
            {
                return CurrentIndex > 0;
                
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
