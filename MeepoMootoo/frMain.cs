using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ts
{
    public partial class frMain : Form
    {
        public static frMain Instance;
        public frMain()
        {
            Instance = this;
            InitializeComponent();
        }

        private void btnJigsaw_Click(object sender, EventArgs e)
        {
            if (panel1.Controls.Count > 0)
            {
                Form a = panel1.Controls[0] as Form;
                a.Close();
            }
            frJigsaw form = new frJigsaw();
    
            this.addForm(form);
        }

        private void btnRubik_Click(object sender, EventArgs e)
        {
            if (panel1.Controls.Count > 0)
            {
                Form a = panel1.Controls[0] as Form;
                a.Close();
            }

            frRubik form = new frRubik();
          
            this.addForm(form);
        }

        private void addForm(Form fr)
        {
            fr.Location = new Point((panel1.Width - fr.Width) / 2, 20);
            fr.TopLevel = false;
            fr.MinimizeBox = false;
            fr.MaximizeBox = false;
            fr.Show();
            //fr.ResumeLayout(false);
            //fr.AutoScroll = true;
            //fr.Dock = DockStyle.Fill;

            fr.FormBorderStyle = FormBorderStyle.None;


            panel1.Controls.Add(fr);
        }

       
    }
}
