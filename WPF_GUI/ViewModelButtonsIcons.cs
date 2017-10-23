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
    }
}
