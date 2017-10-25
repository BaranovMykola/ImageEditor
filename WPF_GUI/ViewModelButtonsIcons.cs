using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPF_GUI.ImageContainer;

namespace WPF_GUI
{
    internal partial class ViewModel
    {
        public ImageSource LeftIcoGray { get; set; } = Icons.left_gray.ToImageSource();

        public ImageSource LeftIco { get; set; } = Icons.left.ToImageSource();

        public ImageSource RightIco { get; set; } = Icons.right.ToImageSource();

        public ImageSource RightIcoGray { get; set; } = Icons.right_gray.ToImageSource();

        public ImageSource OpenIco { get; set; } = Icons.open.ToImageSource();

        public ImageSource OpenIcoGray { get; set; } = Icons.open_gray.ToImageSource();

        public ImageSource RemoveIco { get; set; } = Icons.remove.ToImageSource();

        public ImageSource RemoveIcoGray { get; set; } = Icons.remove_gray.ToImageSource();

        public ImageSource ContrastIco { get; set; } = Icons.contandbirght.ToImageSource();

        public ImageSource ContrastIcoGray { get; set; } = Icons.contandbirght_gray.ToImageSource();

        public ImageSource SaveIco { get; set; } = Icons.save.ToImageSource();

        public ImageSource SaveIcoGray { get; set; } = Icons.save_gray.ToImageSource(); 
    }
}
