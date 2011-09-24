using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;

namespace AutoSlidingTile
{
    //class DefinePieces :AppState
    class DefinePieces 
    {
        List<Point> points = new List<Point>();
        Bitmap[] images;

        //public DefinePieces(MainProgram parent, Bitmap[] images) : base (parent)
        //{
        //    this.images = images;
        //}

        public DefinePieces()
        {

        }

        //internal override void OnUnload()
        //{

        //}

        //internal override void OnLoad()
        //{
        //    UpdateUI();
        //}

        //private void UpdateUI()
        //{
        //    parent.toolPanel.MsgLabel.Text = "#" + (points.Count+1);

        //    if (points.Count >= images.Length)
        //        parent.toolPanel.ImageLabel.BackgroundImage = null;
        //    else
        //        parent.toolPanel.ImageLabel.BackgroundImage = images[points.Count];
        //}

        //internal override void OnFullScreenMouseDown(MouseEventArgs e)
        //{

        //}

        //internal override void OnFullScreenMouseUp(MouseEventArgs e)
        //{
        //    points.Add(new Point(e.X, e.Y));
        //    parent.fullScreen.Invalidate();

        //    UpdateUI();
        //}

        //internal override void OnFullScreenMouseMove(MouseEventArgs e)
        //{

        //}

        //Font font = new Font("San Serif", 25, FontStyle.Bold);
        //Brush brush = new SolidBrush(Color.Green);
        //internal override void OnFullScreenPaint(PaintEventArgs e)
        //{
        //    for (int i = 0; i < points.Count; i++)
        //    {
        //        string str = "" + (i + 1);
        //        SizeF s = e.Graphics.MeasureString(str,font);
        //        e.Graphics.DrawString(str, font, brush, points[i].X - (int)(s.Width/2), points[i].Y - (int)(s.Height/2));
        //    }
        //}

        //public int ComparePointY(Point a, Point b)
        //{
        //    if (a.Y < b.Y)
        //        return -1;
        //    else
        //        return 1;
        //}

        public static int ComparePointX(Point a, Point b)
        {
            if (a.X < b.X)
                return -1;
            else
                return 1;
        }

        //internal override void OnStartButtonClicked()
        //{
        //    if (points.Count != 9 && points.Count != 16)
        //    {
        //        MessageBox.Show("You need to define either 9 or 16 points.");
        //        return;
        //    }

        //    parent.fullScreen.WindowState = FormWindowState.Minimized;

        //    int d = (points.Count == 9) ? 3 : 4;

        //    Point[] sortedPoints = new Point[d * d];
        //    for (int i = 0; i < sortedPoints.Length; i++) sortedPoints[i] = new Point(points[i].X, points[i].Y);

        //    Array.Sort<Point>(sortedPoints, new Comparison<Point>(ComparePointY));

        //    for (int i = 0; i < d; i++)
        //    {
        //        Array.Sort<Point>(sortedPoints, i*d, d, new ComparePointX());
        //    }

        //    int[] pieces = new int[d * d];

        //    for (int i = 0; i < points.Count; i++)
        //    {
        //        int position = -1;
        //        for (int j = 0; j < sortedPoints.Length; j++)
        //        {
        //            if (sortedPoints[j].X == points[i].X
        //                && sortedPoints[j].Y == points[i].Y)
        //            {
        //                position = j;
        //                break;
        //            }
        //        }

        //        pieces[position] = i;
        //    }

        //    points.Clear();
        //    parent.fullScreen.Invalidate();

        //    parent.toolPanel.MsgLabel.Text = "Calculating ...";

        //    long time = DateTime.Now.Ticks;
        //    int[] steps = Solver.Process(d, pieces);

        //    int duration = int.Parse(parent.toolPanel.DurationTextBox.Text);
        //    for (int i = 0; i < steps.Length; i++)
        //    {
        //        SetCursorPos(sortedPoints[steps[i]].X, sortedPoints[steps[i]].Y);

        //        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        //        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

        //        Thread.Sleep(duration);
        //    }

        //    parent.toolPanel.MsgLabel.Text = (((DateTime.Now.Ticks - time) / 10000000L) + "s. (" + steps.Length + " steps)");

        //    parent.State = new DefineOriginalImage(parent);
        //}

        //internal override void OnResetButtonClicked()
        //{
        //    parent.State = new DefineOriginalImage(parent);
        //}

        //internal override void OnGotFocus()
        //{

        //}

        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        public const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        public const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    }

    class ComparePointX : IComparer<Point>
    {

        #region IComparer<Point> Members

        public int Compare(Point x, Point y)
        {
            if (x.X < y.X) return -1;
            else return 1;
        }

        #endregion
    }
}
