﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
        CoreWrapper.ImageProc ip = new ImageProc(@"D:\Studying\Programming\ImageEditor\GUI\fox.jpg");
        private int b = 0;
        public MainWindow()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
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
            var a = ip.readOriginalWrapper(@"D:\Studying\Programming\ImageEditor\GUI\fox.jpg");
            pictureBox1.Image = a;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //ip.editImage(1,0,1,trackBar1.Value-255);
            //var a = ip.readOriginalWrapper(@"D:\Studying\Programming\ImageEditor\GUI\i.jpg");
            //pictureBox1.Image = a;

        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            ip.editContrastAndBrightness((float) (trackBar2.Value/100.0), trackBar1.Value-255);
            var a = ip.getPreview(600,600);
            pictureBox1.Image = a;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            var sw = new Stopwatch();
            sw.Start();
            var t = new Thread(() => changeImg(ip, pictureBox1));
            t.Start();
            t.Join();
            sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds);
            label1.Text = (b++).ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            ip.rotateImage(trackBar3.Value);
            var a = ip.getPreview(600, 600);
            pictureBox1.Image = a;
        }

        private void changeImg(ImageProc _ip, PictureBox _pictureBox1)
        {
            _ip.editContrastAndBrightness((float)(trackBar2.Value / 100.0), trackBar1.Value - 255);
            var a = ip.getPreview(9000, 9000);
            _pictureBox1.Image = a;
        }
    }
}
