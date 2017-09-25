using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF_GUI.ImageContainer
{
    class ImageStorage
    {
        private List<Uri> imageSourses = new List<Uri>();
        public int CurrentIndex { get; set; } = 0;

        public Action LockLeft;
        public Action LockRight;
        public Action LockRemove;
        public Action UnlockAll;
        //public Action<string[]> LoadPreview;

        public ImageStorage()
        {
            
        }

        public ImageStorage(string[] pathes)
        {
            LoadImages(pathes);
        }

        public void LoadImages(string[] pathes)
        {
            foreach (var path in pathes)
            {
                imageSourses.Add(new Uri(path, UriKind.Absolute));
            }
            //LoadPreview(pathes);
        }

        public BitmapImage Current
        {
            get
            {
                UnlockAll();
                if (CurrentIndex == 0)
                {
                    LockLeft();
                }
                if (CurrentIndex + 1 == imageSourses.Count)
                {
                    LockRight();
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
                    LockLeft();
                    LockRight();
                    LockRemove();
                    return new BitmapImage();
                }
            }
        }

        public BitmapImage Next
        {
            get
            {
                ++CurrentIndex;
                return Current;
            }
        }

        public BitmapImage Prev
        {
            get
            {
                --CurrentIndex;
                return Current;
            }
        }

        public void Remove()
        {
            if (imageSourses.Count > 0)
            {
                if (imageSourses.Count == 1)
                {
                    LockRemove();
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
    }
}
