using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AutoMouse;
using System.Threading;

namespace TestMouse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Down " +DateTime.Now.Ticks);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Up " + DateTime.Now.Ticks);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Click " + DateTime.Now.Ticks);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            csMouseClick.MoveCursorTo(this.Left + this.Width / 2, this.Top + this.Height / 2, 0, 0);
            new Thread(new ThreadStart(Run)).Start();
        }

        public delegate void d_Show();
        private void Run()
        {
            csMouseClick.mouse_event(csMouseClick.MOUSEEVENTF_MOVE, 1, 1, 0, 0);
            Thread.Sleep(50);
            csMouseClick.mouse_event(csMouseClick.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(50);
            csMouseClick.mouse_event(csMouseClick.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

            this.Invoke(new d_Show(this.Show));
        }
    }
}
