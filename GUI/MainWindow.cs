﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoreWrapper;

//using CoreWrapper;

namespace GUI
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ImageProc ip = new ImageProc();
            //int a = ip.foo();


        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //CoreWrapper.ImageProc ip = new CoreWrapper.ImageProc();
            //ip.foo();
            //CoreWrapper.ImageProc ip = new CoreWrapper.ImageProc();
            //CoreWrapper.ImageProc ip = new ImageProc();
            CoreWrapper.ImageProc ip = new ImageProc(@"D:\Studying\Programming\ImageEditor\GUI\i.jpg");
            var a = ip.readOriginalWrapper(@"D:\Studying\Programming\ImageEditor\GUI\i.jpg");
            pictureBox1.Image = a;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
