using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace KeyManager
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string key = KeyManager.GenerateKey(GetCpuId.GetCpuId.GetId());
            Console.WriteLine(key);

            Clipboard.SetText(key);
            Console.ReadLine();
        }
    }
}
