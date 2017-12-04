namespace WPF_GUI.ViewModel
{
    using System.Windows.Media;
    using WPF_GUI.ImageContainer;

    /// <summary>
    /// Part of ViewModel. Provides icons for binding buttons in MainWindow
    /// </summary>
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

        public ImageSource RotateIco { get; set; } = Icons.rotate.ToImageSource();

        public ImageSource RotateIcoGray { get; set; } = Icons.rotate_gray.ToImageSource();

        public ImageSource ResizeIco { get; set; } = Icons.resize.ToImageSource();

        public ImageSource ResizeIcoGray { get; set; } = Icons.resize_gray.ToImageSource();

        public ImageSource FaceIco { get; set; } = Icons.face.ToImageSource();

        public ImageSource FaceIcoGray { get; set; } = Icons.face_gray.ToImageSource();

        public ImageSource PaletteIco { get; set; } = Icons.palette.ToImageSource();

        public ImageSource PaletteIcoGray { get; set; } = Icons.palette_gray.ToImageSource();

        public ImageSource GrayscaleIco { get; set; } = Icons.grayscale.ToImageSource();

        public ImageSource GrayscaleIcoGray { get; set; } = Icons.grayscale_gray.ToImageSource();

        public ImageSource FilterIco { get; set; } = Icons.filter.ToImageSource();

        public ImageSource FilterIcoGray { get; set; } = Icons.filter_gray.ToImageSource();
    }
}
