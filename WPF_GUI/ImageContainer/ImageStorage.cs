using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WPF_GUI.ImageContainer
{
    class ImageStorage
    {
        private List<Uri> imageSourses = new List<Uri>();
        private int currentSourceIndex = 0;

        public Action LockLeft;
        public Action LockRight;
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
                var b = new BitmapImage();
                b.BeginInit();
                b.UriSource = imageSourses[currentSourceIndex];
                b.EndInit();
                return b;
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
    }
}
