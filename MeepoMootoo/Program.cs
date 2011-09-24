using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace ts
{
    static class Program
    {
        private readonly static string KEY = "%%KEY%%";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!KeyManager.KeyManager.IsKeyValid(KEY))
            {
                MessageBox.Show("The program is not valid for this computer. Please buy new copy.");
                return;
            }

            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            currentDir = currentDir.Replace("file:\\", "");

            string fullKeyFilePath = currentDir + "/key.license";
            string fullParityFilePath = currentDir + "/parity_check.license";
            if (!File.Exists(fullKeyFilePath) || !File.Exists(fullParityFilePath))
            {
                MessageBox.Show("License files are invalid. Either key.license or parity_check.license is not valid or not found.");
                return;
            }

            //Console.WriteLine("It works !");
            //Console.ReadLine();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frMain());
        }
    }
}
