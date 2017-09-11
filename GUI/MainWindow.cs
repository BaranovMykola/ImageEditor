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
using CoreWrapper;

//using CoreWrapper;

namespace GUI
{
    public partial class MainWindow : Form
    {
        CoreWrapper.ImageProc ip = new ImageProc(@"D:\Studying\Programming\ImageEditor\GUI\fox.jpg", 1980, 1080);
        private int b = 0;

        public MainWindow()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
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
    }
}
