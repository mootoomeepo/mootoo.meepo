using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace RotationPuzzleFrame
{
    class csMouseClick
    {
        [DllImport("user32")]
        private static extern int SetCursorPos(int x, int y);

        public static int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        public static int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        public static int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        public static int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
        public static int MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private static Random rand = new Random();
        public static void MoveCursorTo(int x, int y,int fuzzyX,int fuzzyY)
        {
#if ADMIN
            x = x + rand.Next(-fuzzyX, fuzzyX);
            y = y + rand.Next(-fuzzyY, fuzzyY);

            int FACTOR = 50;
            int dx = (x - Cursor.Position.X);

            if (Math.Abs(dx) < FACTOR)
            {
                if (x > Cursor.Position.X) dx = 1;
                else if (x < Cursor.Position.X) dx = -1;
                else dx = 0;
            }
            else
            {
                dx = dx / FACTOR;
            }

            int dy = (y - Cursor.Position.Y);
            if (Math.Abs(dy) < FACTOR)
            {
                if (y > Cursor.Position.Y) dy = 1;
                else if (y < Cursor.Position.Y) dy = -1;
                else dy = 0;
            }
            else
            {
                dy = dy / FACTOR;
            }

            while (true)
            {
                int currentX = Cursor.Position.X;
                int currentY = Cursor.Position.Y;

                int new_dx = (x - currentX);
                int new_dy = (y - currentY);

                if ((new_dx*dx) <= 0) dx = new_dx;
                if ((new_dy * dy) <= 0) dy = new_dy;

                if (dx == 0 && dy == 0) break;

                csMouseClick.SetCursorPos(currentX + dx, currentY + dy);
                Thread.Sleep(1);
            }
#endif

            csMouseClick.SetCursorPos(x, y);
        }
    }
}
