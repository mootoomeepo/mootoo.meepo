using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FeedPuzzler
{
    public partial class frmMainFeedPuzzler : Form
    {
        frmFeedPuzzler_Crop _crop = new frmFeedPuzzler_Crop();


        public frmMainFeedPuzzler()
        {
            InitializeComponent();

            _crop.FormClosed += new FormClosedEventHandler(_crop_FormClosed);

            _crop.Show();
        }

        
        private void btnStart_Click(object sender, EventArgs e)
        {
            _crop._IsForceDoubleClick = checkBox1.Checked;
            _crop.Play((int)(numericUpDown1.Value*1000));
        }

        private void ManualCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _crop.SetManual(ManualCheckBox.Checked);
        }

        void _crop_FormClosed(object sender, FormClosedEventArgs e)
        {
            _crop = new frmFeedPuzzler_Crop();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Visible = true;
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (webBrowser1.Visible == true)
            {
                System.Diagnostics.Process.Start(e.Url.ToString());
                e.Cancel = true;
            }
        }
    }
}
