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
        private int currentSourceIndex = 0;

        public Action LockLeft;
        public Action LockRight;
        public Action LockRemove;
        public Action UnlockAll;

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
        }

        public BitmapImage Current
        {
            get
            {
                UnlockAll();
                if (currentSourceIndex == 0)
                {
                    LockLeft();
                }
                if (currentSourceIndex + 1 == imageSourses.Count)
                {
                    LockRight();
                }
                if (imageSourses.Count != 0)
                {
                    var b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = imageSourses[currentSourceIndex];
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
                ++currentSourceIndex;
                return Current;
            }
        }

        public BitmapImage Prev
        {
            get
            {
                --currentSourceIndex;
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
                imageSourses.RemoveAt(currentSourceIndex);
                if (imageSourses.Count > 0)
                {
                    if (currentSourceIndex == imageSourses.Count)
                    {
                        --currentSourceIndex;
                    }
                    if (currentSourceIndex == -1)
                    {
                        ++currentSourceIndex;
                    }
                }
            }
        }
    }
}
