using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestLib;

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
            TestLib.Class1 c = new Class1();
        }
    }
}
