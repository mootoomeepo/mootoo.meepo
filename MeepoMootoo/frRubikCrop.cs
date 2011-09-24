using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Imaging;
using System.Threading;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AutoMouse;

namespace ts
{
    public partial class frRubikCrop : Form
    {
        Thread Worker;

        int _Duration = 0;

        int _UpDownArrowWidth = 30;
        int _UpDownArrowHeight = 20;
        int _LeftRightArrowWidth = 20;
        int _LeftRightArrowHeight = 30;

        int[,] _Matrix = new int[9, 9];
        /* empty = 0
         * red = 1
         * blue = 2
         * green = 3
         * purple = 4
         * yellow = 5
        */
        int[] _Move = new int[]{0, 1, 2};
        string[] _MoveDir = new string[] { "U", "D", "L", "R" };
        string[] _ClickStep;
        Point[] _ClickPosition;

        int _InnerBlockSize;
        int _BlockBorder = 1;



        //red, blue, green, purple, yellow
        Bitmap[] _Block = new Bitmap[5];

        ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0);

        public frRubikCrop()
        {
            InitializeComponent();

            //this.setControl();
        }

        private void frRubikCrop_Load(object sender, EventArgs e)
        {
            _InnerBlockSize = int.Parse(textBox1.Text);
            _Block[0] = this.ResizeImage(new Bitmap(Application.StartupPath + "/Images/Red.png"), _InnerBlockSize / 2);
            _Block[1] = this.ResizeImage(new Bitmap(Application.StartupPath + "/Images/Blue.png"), _InnerBlockSize / 2);
            _Block[2] = this.ResizeImage(new Bitmap(Application.StartupPath + "/Images/Green.png"), _InnerBlockSize / 2);
            _Block[3] = this.ResizeImage(new Bitmap(Application.StartupPath + "/Images/Purple.png"), _InnerBlockSize / 2);
            _Block[4] = this.ResizeImage(new Bitmap(Application.StartupPath + "/Images/Yellow.png"), _InnerBlockSize / 2);

            this.setControl();    
        }

        private void PrepareData()
        {
            _InnerBlockSize = int.Parse(textBox1.Text);

            flowLayoutPanel1.Height = _InnerBlockSize;

  
            foreach (Bitmap bm in _Block)
            {
                flowLayoutPanel1.Controls.Add(this.genPic(bm, _InnerBlockSize));
            }

            this.setControl();

            for (int i = 0; i < System.Math.Sqrt(_Matrix.Length); i++)
            {
                for (int j = 0; j < System.Math.Sqrt(_Matrix.Length); j++)
                {
                    _Matrix[i, j] = 0;
                }
            }

            //Define ClickPostion

            _ClickStep = new string[_Move.Length * _MoveDir.Length];
            _ClickPosition = new Point[_ClickStep.Length];

            for (int i = 0; i < _Move.Length; i++)
            {
                for (int j = 0; j < _MoveDir.Length; j++)
                {
                    _ClickStep[(_MoveDir.Length * i) + j] = _Move[i].ToString() + _MoveDir[j];

                    if (_MoveDir[j] == "U")
                    {
                        _ClickPosition[(_MoveDir.Length * i) + j] = new Point((panel1.PointToScreen(c.Location).X + (_UpDownArrowWidth / 2)) + (i * _InnerBlockSize), (panel1.PointToScreen(c.Location).Y - (_UpDownArrowHeight / 2) - (_BlockBorder*2)));
                    }
                    else if (_MoveDir[j] == "D")
                    {
                        _ClickPosition[(_MoveDir.Length * i) + j] = new Point((panel1.PointToScreen(c.Location).X + (int)(_UpDownArrowWidth *1.5)) + (i * _InnerBlockSize), (panel1.PointToScreen(c.Location).Y - (_UpDownArrowHeight / 2) - (_BlockBorder * 2)));
                    }
                    else if (_MoveDir[j] == "L")
                    {
                        _ClickPosition[(_MoveDir.Length * i) + j] = new Point((panel1.PointToScreen(c.Location).X - (_LeftRightArrowWidth / 2)), (panel1.PointToScreen(c.Location).Y + (_LeftRightArrowHeight/2)) + (i * _InnerBlockSize));
                    }
                    else if (_MoveDir[j] == "R")
                    {
                        _ClickPosition[(_MoveDir.Length * i) + j] = new Point((panel1.PointToScreen(c.Location).X - (_LeftRightArrowWidth / 2)), (panel1.PointToScreen(c.Location).Y + (int)(_LeftRightArrowHeight * 1.5)) + (i * _InnerBlockSize));
                    }
                }
            }
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            this.setControl();
        }

        private PictureBox genPic(Bitmap bm, int Width)
        {
            PictureBox pb = new PictureBox();
            pb.Image = bm;
            pb.Size = new Size(Width, Width);
            pb.Padding = new Padding(0);
            pb.Margin = new Padding(0);

            return pb;
        }

        private void setControl()
        {
            _InnerBlockSize = int.Parse(textBox1.Text);
            vt1.Size = vt2.Size = vt3.Size = vd1.Size = vd2.Size = vd3.Size = new Size(_InnerBlockSize/2, _InnerBlockSize/2 * 3);

            vt2.Location = new Point((panel1.Width/2) - (_InnerBlockSize/4), 0);
            vt1.Location = new Point(vt2.Location.X - (_BlockBorder * 2) - (_InnerBlockSize), 0);
            vt3.Location = new Point(vt2.Location.X + (_BlockBorder * 2) + (_InnerBlockSize), 0);

            vd2.Location = new Point(vt2.Location.X, panel1.Height - (vt1.Size.Height));
            vd1.Location = new Point(vt1.Location.X, vd2.Location.Y);
            vd3.Location = new Point(vt3.Location.X, vd2.Location.Y);

            hl1.Size = hl2.Size = hl3.Size = hr1.Size = hr2.Size = hr3.Size = new Size(_InnerBlockSize/2 * 3, _InnerBlockSize/2);

            hl2.Location = new Point(0, (panel1.Height / 2) - (_InnerBlockSize / 4));
            hl1.Location = new Point(0, hl2.Location.Y - (_BlockBorder * 1) - _InnerBlockSize);
            hl3.Location = new Point(0, hl2.Location.Y + (_BlockBorder * 2) + _InnerBlockSize);

            hr2.Location = new Point(panel1.Width -(_InnerBlockSize/2 *3) , hl2.Location.Y);
            hr1.Location = new Point(hr2.Location.X, hl1.Location.Y);
            hr3.Location = new Point(hr2.Location.X, hl3.Location.Y);

            c.Size = new Size(_InnerBlockSize * 3, _InnerBlockSize * 3);
            c.Location = new Point((int)((panel1.Width / 2) - (_InnerBlockSize / 2 * 3)) - (_BlockBorder * 2), (int)((panel1.Height / 2) - (_InnerBlockSize / 2 * 3)));
        }

        private Bitmap ResizeImage(Bitmap img, int Widht)
        {
            Bitmap a = new Bitmap(img, Widht, Widht);
            return a.Clone(new Rectangle(0, 0, Widht, Widht), PixelFormat.Format24bppRgb);
        }

        private string showMatrix()
        {
            string tmp = string.Empty;

            for (int i = 0; i < System.Math.Sqrt(_Matrix.Length); i++)
            {
                for (int j = 0; j < System.Math.Sqrt(_Matrix.Length); j++)
                {
                    tmp += _Matrix[i, j] + " , ";
                }

                tmp += Environment.NewLine;
            }

            return tmp;
        }

        private int CompareBlock(Bitmap Image)
        {
            Double[] score = new double[_Block.Length];

            double max = Double.MinValue;
            for (int i = 0; i < _Block.Length; i++)
            {
                TemplateMatch[] matchings = tm.ProcessImage(Image, _Block[i]);
                score[i] = matchings[0].Similarity;

                if (max < score[i])
                {
                    max = score[i];
                }
            }

            return Array.IndexOf(score, max) + 1;
        }

        private Bitmap getBlockImage(int BlockIndex)
        {
            return _Block[BlockIndex - 1];
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            //this.playRubik(20, 400, 40000);
        }

        bool _isForceDoubleClick;
        public void playRubik(int maxStep, int Duration, int TimeLimitMs, bool isForceDoubleClick)
        {
            _isForceDoubleClick = isForceDoubleClick;

            this.PrepareData();
            //
            this.Hide();
            //frMain.Instance.WindowState = FormWindowState.Minimized;

            _Duration = Duration;

            Bitmap screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format24bppRgb);
            Graphics gfxScreenshot = Graphics.FromImage(screenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            this.Hide();

            List<FlowLayoutPanel> pn = new List<FlowLayoutPanel>();

            for (int i = 0; i < panel1.Controls.Count; i++)
            {
                if (panel1.Controls[i] is FlowLayoutPanel)
                {
                    pn.Add((FlowLayoutPanel)(panel1.Controls[i]));
                }
            }

            foreach (FlowLayoutPanel p in pn)
            {
                p.Controls.Clear();

                int BlockIndex = -1;
                if (p.Width > p.Height)
                {// Horizontal
                    // use Index naja
                    int matrixCol = p.Location.X == 0 ? 0 : 6;
                    int matrixRow = 2 + int.Parse(p.Name.Substring(p.Name.Length - 1, 1));

                    for (int i = 0; i < 3; i++)
                    {
                        //p.Controls.Add(
                        //    this.genPic(
                        //    this.getBlockImage(
                        //    BlockIndex = this.CompareBlock
                        //    (screenshot.Clone(new Rectangle(this.PointToScreen(p.Location).X + panel1.Location.X + (i * _InnerBlockSize / 2), this.PointToScreen(p.Location).Y + panel1.Location.Y, _InnerBlockSize / 2, _InnerBlockSize / 2), screenshot.PixelFormat))
                        //    )
                        //    , _InnerBlockSize / 2)
                        //    );
                        //_Matrix[matrixRow, matrixCol + i] = BlockIndex;
                        //this.Refresh();
                        _Matrix[matrixRow, matrixCol + i] = this.CompareBlock
                            (screenshot.Clone(new Rectangle(this.PointToScreen(p.Location).X + panel1.Location.X + (i * _InnerBlockSize / 2), this.PointToScreen(p.Location).Y + panel1.Location.Y, _InnerBlockSize / 2, _InnerBlockSize / 2), screenshot.PixelFormat));
                     }
                }
                else if (p.Width < p.Height)
                {//Vertical
                    int matrixCol = 2 + int.Parse(p.Name.Substring(p.Name.Length - 1, 1));
                    int matrixRow = p.Location.Y == 0 ? 0 : 6;

                    for (int i = 0; i < 3; i++)
                    {
                        //p.Controls.Add(
                        //    this.genPic(
                        //    this.getBlockImage(
                        //    BlockIndex = this.CompareBlock
                        //    (screenshot.Clone(new Rectangle(this.PointToScreen(p.Location).X + panel1.Location.X, this.PointToScreen(p.Location).Y + panel1.Location.Y + (i * _InnerBlockSize / 2), _InnerBlockSize / 2, _InnerBlockSize / 2), screenshot.PixelFormat))
                        //    )
                        //    , _InnerBlockSize / 2)
                        //    );

                        //_Matrix[matrixRow + i, matrixCol] = BlockIndex;
                        //this.Refresh();

                        _Matrix[matrixRow + i, matrixCol] = this.CompareBlock
                            (screenshot.Clone(new Rectangle(this.PointToScreen(p.Location).X + panel1.Location.X, this.PointToScreen(p.Location).Y + panel1.Location.Y + (i * _InnerBlockSize / 2), _InnerBlockSize / 2, _InnerBlockSize / 2), screenshot.PixelFormat));
                    }
                }
                else
                {//Center
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            //p.Controls.Add(
                            //       this.genPic(
                            //       this.ResizeImage(
                            //       this.getBlockImage(
                            //       BlockIndex = this.CompareBlock(
                            //       screenshot.Clone(new Rectangle(this.PointToScreen(p.Location).X + panel1.Location.X + (j * _InnerBlockSize), this.PointToScreen(p.Location).Y + panel1.Location.Y + (i * _InnerBlockSize), _InnerBlockSize, _InnerBlockSize), screenshot.PixelFormat))
                            //       )
                            //       , _InnerBlockSize)
                            //       , _InnerBlockSize)
                            //       );

                            //_Matrix[3 + i, 3 + j] = BlockIndex;
                            //this.Refresh();
                            _Matrix[3 + i, 3 + j]= this.CompareBlock(
                                   screenshot.Clone(new Rectangle(this.PointToScreen(p.Location).X + panel1.Location.X + (j * _InnerBlockSize), this.PointToScreen(p.Location).Y + panel1.Location.Y + (i * _InnerBlockSize), _InnerBlockSize, _InnerBlockSize), screenshot.PixelFormat));
                                   
                        }
                    }
                }
            }

            this.Hide();

            int countStep = 0;
            steps = ColorBlock.AvgPathColorBlockSolver.Solve(_Matrix, ref countStep, maxStep, (long)Duration,(long) TimeLimitMs);

            if (steps == null)
            {
                MessageBox.Show("จอมยุทธน้อยช่วยท่านไม่ได้ ท่านต้องเล่นเองแล้วแหละ !!");
                steps = new string[0];
                return;
            }
            isClicking = true;
            Worker = new Thread(this.endRubik);
            Worker.Start();
            
            //MessageBox.Show(this.showMatrix());
        }

        private bool isClicking = false;
        string[] steps = new string[0];
        Random rand = new Random();
        private void endRubik()
        {
            for (int i = 0; i < steps.Length; i++)
            {
                if (isClicking == false) return;

                int x = _ClickPosition[Array.IndexOf(_ClickStep, steps[i])].X;
                int y = _ClickPosition[Array.IndexOf(_ClickStep, steps[i])].Y;

                csMouseClick.MoveCursorTo(x, y , 0, 0);

                csMouseClick.LeftClick(_Duration);

                Thread.Sleep(_Duration);
            }

            if (isClicking == false) return;

            // repeat the last click
            if (_isForceDoubleClick && steps.Length > 0)
            {
                int fuzzyX = 0;
                int fuzzyY = 0;

                //string d = steps[steps.Length - 1].Substring(1, 1);

                //if (d == "L" || d == "R") fuzzyY = 10;
                //else fuzzyX = 10;

                int x = _ClickPosition[Array.IndexOf(_ClickStep, steps[steps.Length - 1])].X;
                int y = _ClickPosition[Array.IndexOf(_ClickStep, steps[steps.Length - 1])].Y;

                csMouseClick.MoveCursorTo(x, y, 0, 0);

                csMouseClick.LeftClick(_Duration);
            }

            //this.showMatrix();
            steps = new string[0];
        }

        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (Worker != null)
            {
                if (e.KeyCode.ToString().ToUpper() == "SPACE")
                {
                    isClicking = false;

                    this.Show();
                }
            }
        }

        private void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            //textBoxLog.AppendText(string.Format("MouseDown - {0}\n", e.Button));
            //textBoxLog.ScrollToCaret();

            if (Worker != null)
            {
                if (e.Button.ToString() == "Right")
                {
                    isClicking = true;

                    this.Show();
                }
            }
        }


    }
}
