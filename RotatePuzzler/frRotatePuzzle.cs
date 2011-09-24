using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Imaging;
using AForge.Imaging;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

using AutoMouse;

namespace RotationPuzzleFrame
{
    public partial class frRotatePuzzle : Form
    {
        Size _fixPerfectImageSize = new Size(128, 128);
        Size _fixPuzzleImageSize = new Size(385,385);

        Bitmap screenshot;

        Form frPerfectImage;
        Form frPuzzleImage;
        Size _initSize;
        bool _NeedHelp = false;
        int _EmptySlotNumber = -1;

        string[] steps;
        Point[] ClickPosition;
        int duration;

        public frRotatePuzzle()
        {
            this.InitializeComponent();

            this.createFormPuzzleImage();
            this.createFormPerfectImage();

            _initSize = this.Size;

            webBrowser1.Visible = false;
        }

        private void createFormPerfectImage()
        {
            frPerfectImage = new Form();
            
            frPerfectImage.MaximizeBox = false;
            frPerfectImage.MinimizeBox = false;
            frPerfectImage.Text = "Perfect Image";
            frPerfectImage.Opacity = 0.7f;
            frPerfectImage.Show();
            frPerfectImage.FormBorderStyle = FormBorderStyle.Sizable;
            frPerfectImage.Size = _fixPerfectImageSize;
            frPerfectImage.MinimumSize = new Size(10, 10);

            frPerfectImage.FormClosed += new FormClosedEventHandler(frPerfectImage_FormClosed);
        }

        private void createFormPuzzleImage()
        {
            frPuzzleImage = new Form();
            
            frPuzzleImage.MaximizeBox = false;
            frPuzzleImage.MinimizeBox = false;
            frPuzzleImage.Text = "Puzzle Image";
            frPuzzleImage.Opacity = 0.7f;
            frPuzzleImage.Show();
            frPuzzleImage.FormBorderStyle = FormBorderStyle.Sizable;
            frPuzzleImage.Size = _fixPuzzleImageSize;

            frPuzzleImage.FormClosed += new FormClosedEventHandler(frPuzzleImage_FormClosed);
        }

        void frPuzzleImage_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.createFormPuzzleImage();
        }

        void frPerfectImage_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.createFormPerfectImage();
        }

        Thread Worker;

        private void frRotatePuzzle_Load(object sender, EventArgs e)
        {
            Gma.UserActivityMonitor.HookManager.MouseDown += HookManager_MouseDown;
            Gma.UserActivityMonitor.HookManager.KeyDown += HookManager_KeyDown;


#if TRIAL
            MessageBox.Show("Trial Version เล่นได้เฉพาะ 3*3 เท่านั้น");
#else
            
#endif
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            csMouseClick.MoveCursorTo(0, 0, 0, 0);
            this.Cursor = Cursors.WaitCursor;

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();

            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0);

            frPerfectImage.Hide();
            frPuzzleImage.Hide();

            //Cap a screenshot
            screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format24bppRgb);
            Graphics gfxScreenshot = Graphics.FromImage(screenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            Bitmap[] PiecesPerfectImage = this.getPerfectImagePieces();
            Bitmap[] PiecesPuzzleImage = this.getPuzzleImagePieces();
            ClickPosition = this.getClickArea(PiecesPuzzleImage.Length);

            int width = PiecesPerfectImage[0].Width;
            int Height = PiecesPerfectImage[0].Height;

            for (int i = 0; i < PiecesPuzzleImage.Length; i++)
            {
                PiecesPuzzleImage[i] = new Bitmap(PiecesPuzzleImage[i], PiecesPerfectImage[0].Width, PiecesPerfectImage[0].Height);
                PiecesPuzzleImage[i] = PiecesPuzzleImage[i].Clone(new Rectangle(0, 0, PiecesPerfectImage[i].Width, PiecesPerfectImage[i].Height), PixelFormat.Format24bppRgb);
            }

            string Output = string.Empty;
            string ss = string.Empty;

            int[] PiecesNumber = new int[PiecesPuzzleImage.Length];
            float[] scoreMax = new float[PiecesPuzzleImage.Length];

            //Init All Pieces number = -1 ไว้เช็คภาพที่โดนเลือกไปแล้ว
            for (int i = 0; i < PiecesNumber.Length; i++)
            {
                PiecesNumber[i] = -1;
            }

            //วิธีใหม่
            float[,] scoreAll = new float[PiecesPuzzleImage.Length, PiecesPuzzleImage.Length];
            int[] PiecesNumberNew = new int[PiecesPuzzleImage.Length];

            for (int i = 0; i < PiecesPuzzleImage.Length; i++)
            {
                for (int j = 0; j < PiecesPerfectImage.Length ; j++)
                {
                    //{//ยังไม่โดนเลือก
                    TemplateMatch[] matchings = tm.ProcessImage(PiecesPuzzleImage[i], PiecesPerfectImage[j]);
                    //score[j] = matchings[0].Similarity;//Max Similarity

                    scoreAll[i, j] = matchings[0].Similarity;
                }//end for (int j = 0; j < PiecesPerfectImage.Length; j++)


                ss += Environment.NewLine;
            }//end for (int i = 0; i < PiecesPuzzleImage.Length; i++)

            //New
            int[] AssignedPieces = new int[PiecesPuzzleImage.Length];

            for (int i = 0; i < AssignedPieces.Length; i++)
            {
                AssignedPieces[i] = -1;
            }

            for (int i = 0; i < AssignedPieces.Length; i++)
            {
                float maxScore = 0f;
                int maxScoreIndex = -1;

                for (int j = 0; j < PiecesPuzzleImage.Length; j++)
                {
                    if (Array.IndexOf(AssignedPieces, j) == -1)
                    {
                        if (scoreAll[i, j] > maxScore)
                        {
                            maxScore = scoreAll[i, j];
                            maxScoreIndex = j;
                        }
                    }
                    else
                    {

                    }
                }
                AssignedPieces[i] = maxScoreIndex;
            }

            long time = DateTime.Now.Ticks;


            try
            {
                int countStep = 0;

                //for (int i = 0; i < AssignedPieces.Length; i++)
                //{
                //    Console.Write(AssignedPieces[i] + ",");
                //}


                steps = RotationPuzzle.RotationPuzzleSolver.Process((int)System.Math.Sqrt(AssignedPieces.Length), (int)System.Math.Sqrt(AssignedPieces.Length), AssignedPieces, ref countStep);
                
                
            }
            catch (Exception ex)
            {//
                //this.Size = new Size(207, 391);

                string tmp = string.Empty;
                int d = (int)System.Math.Sqrt(AssignedPieces.Length);
                for (int i = 0; i < AssignedPieces.Length; i++)
                {
                    
                    tmp += AssignedPieces[i].ToString() + ", ";

                    if ((i + 1) % d == 0)
                    {
                        tmp += Environment.NewLine;
                    }
                }

                if (_NeedHelp)
                {
                    MessageBox.Show("ข้าน้อยไร้ความสามารถ พวกท่านแก้กันเองแล้วกัน !!!!!" + Environment.NewLine + tmp.ToString());
                }
                else
                {
                    _NeedHelp = true;

                    MessageBox.Show("ขอตัวช่วย --> เลือกช่องว่างให้จอมยุทธน้อยทีเถ๊อะ !!!!" + Environment.NewLine + tmp.ToString());
                }
                this.Cursor = Cursors.Default;
                frPerfectImage.Show();
                frPuzzleImage.Show();
                return;
            }

            this.Cursor = Cursors.Default;

            duration = (int)(tbDuration.Value * 1000);
            if (steps != null)
            {
                if (steps.Length == 0) return;

                //    for (int i = 0; i < steps.Length; i++)
                //    {
                //        this.sendMouseClick(ClickPosition[steps[i]].X, ClickPosition[steps[i]].Y, duration);
                //    }

                //    //แก้ปัญหาที่มันค้างในการกดครั้งสุดท้าย
                //    this.sendMouseClick(ClickPosition[steps[steps.Length - 1]].X, ClickPosition[steps[steps.Length - 1]].Y, duration);
                frPerfectImage.Hide();
                frPuzzleImage.Hide();

                isClicking = true;
                Worker = new Thread(this.EndPuzzle);
                Worker.Start();
            }
            else
            {
                string tmp = string.Empty;

                foreach (int a in AssignedPieces)
                {
                    tmp += a.ToString() + ", ";
                }
                this.Cursor = Cursors.Default;
                frPerfectImage.Show();
                frPuzzleImage.Show();
                MessageBox.Show("เจอปัญหาแล้วจ้า !!  steps = null" + Environment.NewLine + tmp.Substring(0, tmp.Length - 2));
            }

            this.ClearForm();

            string pr = string.Empty;
            int dd = (int)System.Math.Sqrt(AssignedPieces.Length);
            for (int i = 0; i < AssignedPieces.Length; i++)
            {
                pr += AssignedPieces[i] + ", ";

                if ((i + 1) % dd == 0)
                    pr += Environment.NewLine;
            }

            label4.Text = pr;
        }

        private bool isClicking = false;
        private void EndPuzzle()
        {
            for (int i = 0; i < steps.Length; i++)
            {
                if (isClicking == false) return;

                Console.WriteLine(steps[i]);

                int pos = int.Parse(steps[i].Substring(0, 2));
                string d = steps[i].Substring(2, 1);

                this.sendMouseClick(ClickPosition[pos].X, ClickPosition[pos].Y, duration, ((d == "T") ? false : true));

                if (cbForceDoubleClick.Checked && i == (steps.Length-1))
                    this.sendMouseClick(ClickPosition[pos].X, ClickPosition[pos].Y, duration, ((d == "T") ? false : true));
            }

            Console.WriteLine("-----");

            isClicking = false;

            //btnStart_Click(null, null);
        }

        Random rand = new Random();
        private void sendMouseClick(int x, int y, int duration, bool isRightClick)
        {
            Console.WriteLine("Click " + x + " " + y + " " + isRightClick + " " + duration);

            csMouseClick.MoveCursorTo(x, y,0,0);
            isRightClick = false;
            if (isRightClick) csMouseClick.RightClick(duration);
            else csMouseClick.LeftClick(duration);

            Thread.Sleep(duration);
        }

        private void ClearForm()
        {
            //frPerfectImage.Show();
            //frPuzzleImage.Show();

            _NeedHelp = false;
            _EmptySlotNumber = -1;

            //if (checkBox1.Checked)
            //{
            //    this.Size = new Size(900, 400);
            //}
            //else
            //{
            //    this.Size = _initSize;
            //}
        }

        private Bitmap[] getPerfectImagePieces()
        {
            return this.getPieces(
                frPerfectImage.Location.X,
                frPerfectImage.Location.Y,
                frPerfectImage.Location.X + frPerfectImage.Width,
                frPerfectImage.Location.Y + frPerfectImage.Height);
        }

        private Bitmap[] getPuzzleImagePieces()
        {
            return this.getPieces(
                frPuzzleImage.Location.X,
                frPuzzleImage.Location.Y,
                frPuzzleImage.Location.X + frPuzzleImage.Width,
                frPuzzleImage.Location.Y + frPuzzleImage.Height);
        }

        private Bitmap[] getPieces(int startX, int startY, int stopX, int stopY)
        {
            if ((stopX - startX) < 50 || (stopY - startY) < 50) return null;

            //Bitmap screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format24bppRgb);
           
            int d = (rb44.Checked) ? 4 : 3;
            Bitmap[] images = new Bitmap[d * d];


            int bw = (stopX - startX) / d;
            int bh = (stopY - startY) / d;

            for (int i = 0; i < images.Length; i++)
            {
                int col = i % d;
                int row = i / d;
                images[i] = screenshot.Clone(new Rectangle(startX + col * bw, startY + row * bh, bw, bh), screenshot.PixelFormat);
            }

            return images;
        }

        private Point[] getClickArea(int NumberOfPuzzle)
        {
            int Width = frPuzzleImage.Width;
            int Height = frPuzzleImage.Height;

            Point TopLeftPosition = frPuzzleImage.Location;

            int d = (int)(System.Math.Sqrt(NumberOfPuzzle)-1);

            Point[] ClickArea = new Point[d*d];
    
            int PieceWidth = Width / (d+1);
            int PieceHeight = Height / (d+1);

            for (int i = 0; i < ClickArea.Length; i++)
            {
                int Row = (i / d) + 1;
                int Column = (i % d) + 1;

                ClickArea[i].X = TopLeftPosition.X + (Column * PieceWidth);
                ClickArea[i].Y = TopLeftPosition.Y + (Row * PieceHeight);
            }

            return ClickArea;
        }

        private Rectangle GetScaledRectangle(System.Drawing.Image img, Rectangle thumbRect)
        {
            if (img.Width < thumbRect.Width && img.Height < thumbRect.Height)
                return new Rectangle(thumbRect.X + ((thumbRect.Width - img.Width) / 2), thumbRect.Y + ((thumbRect.Height - img.Height) / 2), img.Width, img.Height);

            int sourceWidth = img.Width;
            int sourceHeight = img.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)thumbRect.Width / (float)sourceWidth);
            nPercentH = ((float)thumbRect.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            if (destWidth.Equals(0))
                destWidth = 1;
            if (destHeight.Equals(0))
                destHeight = 1;

            Rectangle retRect = new Rectangle(thumbRect.X, thumbRect.Y, destWidth, destHeight);

            if (retRect.Height < thumbRect.Height)
                retRect.Y = retRect.Y + Convert.ToInt32(((float)thumbRect.Height - (float)retRect.Height) / (float)2);

            if (retRect.Width < thumbRect.Width)
                retRect.X = retRect.X + Convert.ToInt32(((float)thumbRect.Width - (float)retRect.Width) / (float)2);

            return retRect;
        }

        private System.Drawing.Image GetResizedImage(System.Drawing.Image img, Rectangle rect)
        {
            Bitmap b = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, 0, 0, rect.Width, rect.Height);
            g.Dispose();

            try
            {
                return (System.Drawing.Image)b.Clone();
            }
            finally
            {
                b.Dispose();
                b = null;
                g = null;
            }
        }

        private void rb33_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton a = sender as RadioButton;

#if TRIAL
            if (a.Name == "rb44" && a.Checked)
            {
                rb33.Checked = true;
                
                MessageBox.Show("Trial Version เล่นได้เฉพาะ 3*3 เท่านั้น");
            }
#else
            //this.genHelpPanel(int.Parse(a.Name.Substring(a.Name.Length - 1, 1)));
#endif
        }

        private void rbEmptySlotNumber_Click(object sender, EventArgs e)
        {
            _EmptySlotNumber = int.Parse((sender as RadioButton).Name);
            this.btnStart_Click(null, null);
        }

        //[DllImport("user32")]
        //public static extern int SetCursorPos(int x, int y);

        //private const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        //private const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        //private const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        //private const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */

        //[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        /// <summary>
        /// mouse hook
        /// </summary>

        private void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            //textBoxLog.AppendText(string.Format("MouseDown - {0}\n", e.Button));
            //textBoxLog.ScrollToCaret();

            //if (Worker != null)
            //{
            //    if (e.Button.ToString() == "Right")
            //    {
            //        isClicking = false;

            //        frPerfectImage.Show();
            //        frPuzzleImage.Show();
            //    }
            //}
        }

        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (Worker != null)
            {
                if (e.KeyCode.ToString().ToUpper() == "SPACE")
                {
                    isClicking = false;

                    frPerfectImage.Show();
                    frPuzzleImage.Show();
                }
            }
        }

        private void frJigsaw_FormClosed(object sender, FormClosedEventArgs e)
        {
            frPerfectImage.Dispose();
            frPuzzleImage.Dispose();
            Worker = null;
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

