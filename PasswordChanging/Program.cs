using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace PasswordChanging
{
    class Program
    {
        static void Main(string[] args)
        {
            string databaseFile = "border_tile_pattern.sqlite";
            string currentPassword = "";
            string newPassword = "mootoo-meepo-wergweguowh2rg134243terbdssdff";

            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            currentDir = currentDir.Replace("file:\\", "");

            string connString = "Data Source=" + currentDir + "/"+databaseFile+";Pooling=True";

            if (currentPassword != "") connString += ";Password="+currentPassword;

            SQLiteConnection conn = new SQLiteConnection(connString);
            conn.Open();

            Console.WriteLine("Changing password ... ");
            if (newPassword == "")
                conn.ChangePassword(new byte[0]);
            else
                conn.ChangePassword(newPassword);

            Console.WriteLine("Done !");
            Console.ReadLine();
        }
    }
}
