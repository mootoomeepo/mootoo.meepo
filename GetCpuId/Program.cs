using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

using System.Windows.Forms;

namespace GetCpuId
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.Run(new GetIdFrame());
        }

    }
}
