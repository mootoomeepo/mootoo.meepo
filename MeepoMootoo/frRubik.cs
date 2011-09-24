using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ts
{
    public partial class frRubik : Form
    {

        frRubikCrop frRubikCrop;
        public frRubik()
        {
            InitializeComponent();

            frRubikCrop = new frRubikCrop();
            frRubikCrop.Show();

            webBrowser1.Visible = false;

#if TRIAL
            MessageBox.Show("Trial version จะไม่สามารถกำหนดจำนวนครั้งสูดสุดได้");
            tbStep.Visible = false;
#else

#endif
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
#if TRIAL
            frRubikCrop.playRubik(25, (int)(tbDuration.Value * 1000), (int)(tbTimeLimit.Value * 1000), cbForceDoubleClick.Checked);

#else
            frRubikCrop.playRubik((int)tbStep.Value, (int)(tbDuration.Value * 1000), (int)(tbTimeLimit.Value * 1000), cbForceDoubleClick.Checked);

            if (cbAutoDecrement.Checked)
            {
                if (tbStep.Value > 1)
                    tbStep.Value--;
            }
#endif
            
            
        }

        private void frRubik_Load(object sender, EventArgs e)
        {
        }

        private void frRubik_FormClosed(object sender, FormClosedEventArgs e)
        {
            frRubikCrop.Dispose();
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
