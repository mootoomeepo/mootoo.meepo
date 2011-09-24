using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GetCpuId
{
    public partial class GetIdFrame : Form
    {
        public GetIdFrame()
        {
            InitializeComponent();

            string id = GetCpuId.GetId();
            textBox1.Text = id;

            Clipboard.SetText(id);

            textBox1.SelectAll();
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.SelectAll();
        }

        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox1.SelectAll();
        }
    }
}
