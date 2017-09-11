using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using CoreWrapper;
using GUI.Properties;

//using CoreWrapper;

namespace GUI
{
    public partial class MainWindow : Form
    {
        enum ImageOrder
        {
            NEXT = 1,
            PREV = -1
        };
        CoreWrapper.ImageProc ip = new ImageProc(@"D:\Studying\Programming\ImageEditor\GUI\fox.jpg", 1980, 1080);
        OpenFileDialog newImage = new OpenFileDialog();
        private Image currentImage = null;
        private string[] imageListPath = null;
        private int currentImageIndex = -1;

        public MainWindow()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            newImage.CheckFileExists = true;
            newImage.CheckPathExists = true;
            newImage.DefaultExt = "*.jpeg;*.jpg";
            newImage.Multiselect = true;

            //tools.ItemSelectionChanged += yourListView_ItemSelectionChanged;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.Columns.Clear();
            listView1.Columns.Add("111", "111", 200);
            //listView1.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoSize = true;

            ImageList lst = new ImageList();
            var files = Directory.GetFiles(@"D:\Studying\Programming\ImageEditor\GUI\testIcons\");
            lst.ImageSize = new Size(100,100);
            int i = 0;
            foreach (var file in files)
            {
                lst.Images.Add(Image.FromFile(file));
            }
            listView1.SmallImageList = lst;
            foreach (var file in files)
            {
                
                listView1.Items.Add(file, ++i);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newImage.ShowDialog();
            imageListPath = newImage.FileNames;
            Console.WriteLine(imageListPath);
            if (imageListPath.Length > 0)
            {
                try
                {
                    currentImageIndex = -1;
                    changeImage(ImageOrder.NEXT);
                    //currentImage = Image.FromFile(imagePath);
                    //pictureBox1.Image = currentImage;
                }
                catch (Exception)
                {
                    MessageBox.Show(Resources.image_error_message);
                }
            }
        }

        private void tools_KeyDown(object sender, KeyEventArgs e)
        {
            tools.FocusedItem.Focused = false;
            Console.WriteLine("Key pressed");
            Console.WriteLine((char)e.KeyValue);
            switch (e.KeyValue)
            {
                case 39:
                    changeImage(ImageOrder.NEXT);
                    break;
                case 37:
                    changeImage(ImageOrder.PREV);
                    break;
            }
        }

        private void changeImage(ImageOrder order)
        {
            var lst = tools.Items;
            var first = lst[0];
            var last = lst[lst.Count - 1];
            setEnabledRowIcon(first, true, 0);
            setEnabledRowIcon(last, true, 1);

            if (currentImageIndex + (int) order >= 0 && currentImageIndex + (int)order < imageListPath.Length)
            {
                currentImageIndex += (int)order;
                pictureBox1.Image = Image.FromFile(imageListPath[currentImageIndex]);
            }

            if (currentImageIndex + (int) order < 0)
            {
                setEnabledRowIcon(first, false, 0);
            }

            if (currentImageIndex + (int) order >= imageListPath.Length)
            {
                setEnabledRowIcon(last, false, 1);
            }
        }

        private void setEnabledRowIcon(ListViewItem item, bool enabled, int imgIndex)
        {
            int newIndex = item.ImageIndex;
            if (enabled)
            {
                newIndex -= 2;
            }
            else
            {
                newIndex += 2;
            }

            if (newIndex >= 0 && newIndex < 4)
            {
                item.ImageIndex = newIndex;
            }
        }

        private void tools_SelectedIndexChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            Console.WriteLine("selected index changed");
            var a = e as ListViewItemSelectionChangedEventArgs;
        }

        private void tools_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            e.Item.Selected = false;
        }
    }
}
