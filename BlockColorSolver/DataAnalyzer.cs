using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using System.Collections;
using System.Threading;

namespace ColorBlock
{
    class Program
    {

        public static long count;
        public static int lastLevel;
        public static Queue<LookupMove> q;
        public static bool isRunning = false;
        static void Main(string[] args)
        {
            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            currentDir = currentDir.Replace("file:\\", "");

            string connString = "Data Source=" + currentDir + "/pattern.sqlite;Pooling=True";
            SQLiteConnection conn = new SQLiteConnection(connString);
            conn.Open();

            for (int i = 0; i < 6; i++)
            {
                int num = (int)(GetNum(i, conn));
                int[,] pos = new int[9,9];

                Console.WriteLine("[" + i + " steps] ("+num+")");
                for (int j=0;j<num;j++)
                {
                    LookupMove m = GetRecord(i,j,conn);

                    for (int k=0;k<9;k++)
                        for (int l=0;l<9;l++)
                            pos[k,l] += m.p[k,l];
                }

                
                for (int k = 0; k < 9; k++)
                {
                    for (int l = 0; l < 9; l++)
                    {
                        if (((0 <= k && k < 3) || (6 <= k && k < 9))
                            && (l < 3 || l >= 6))
                        {
                            Console.Write("\t");
                            continue;
                        }

                        int value = (int)Math.Ceiling(((double)pos[k, l] * 10) / num);
                        if (value == 0)
                            Console.Write("-\t");
                        else
                            Console.Write(value + "\t");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            }

            Console.WriteLine("DONE");
            Console.ReadLine();
        }

        private static long GetNum(int steps,SQLiteConnection conn)
        {
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(1) FROM block_colors WHERE num_steps='"+steps+"'";

            long count = (long)(cmd.ExecuteScalar());

            cmd.Dispose();

            return count;
        }

        private static LookupMove GetRecord(int steps,int index,SQLiteConnection conn)
        {
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM block_colors WHERE num_steps='"+steps+"' ORDER BY pos ASC LIMIT "+index+",1";

            DbDataReader reader = cmd.ExecuteReader();

            LookupMove m = null;
            if (reader.Read())
            {
                string pos = (string)reader["pos"];

                char[] c = pos.ToCharArray();

                int[,] p = new int[9,9];

                int run = 0;

                for (int i = 0; i < 3; i++)
                    for (int j = 3; j < 6; j++)
                        p[i, j] = (int)c[run++] - (int)'0';

                for (int i = 3; i < 6; i++)
                    for (int j = 0; j < 9; j++)
                        p[i, j] = (int)c[run++] - (int)'0';

                for (int i = 6; i < 9; i++)
                    for (int j = 3; j < 6; j++)
                        p[i, j] = (int)c[run++] - (int)'0';

                m = new LookupMove(p, (string)reader["move"], (int)((long)(reader["num_steps"])));
            }

            reader.Dispose();
            cmd.Dispose();

            return m;
        }

        class OutputClass
        {
            public void output()
            {
                while (isRunning)
                {
                    Console.Clear();
                    Console.WriteLine("db=" + Program.count + " level=" + Program.lastLevel);
                    Thread.Sleep(1000);
                }

            }
        }

        public static void ExecuteDbCommand(string sql, SQLiteConnection conn)
        {
            //log.Info("SqlCommand " + sql);

            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public class LookupMove
        {
            public int[,] p;
            public string move;
            public int level;

            public string pos;

            public LookupMove(int[,] p, string move, int level)
            {
                this.level = level;

                this.p = p;
                this.move = move;
                pos = GenPos();
            }

            private string GenPos()
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < 3; i++)
                    for (int j = 3; j < 6; j++)
                        sb.Append(p[i, j]);

                for (int i = 3; i < 6; i++)
                    for (int j = 0; j < 9; j++)
                        sb.Append(p[i, j]);

                for (int i = 6; i < 9; i++)
                    for (int j = 3; j < 6; j++)
                        sb.Append(p[i, j]);

                return sb.ToString();
            }


            public LookupMove next(int index, char direction)
            {
                int[,] newP = new int[9, 9];
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        newP[i, j] = p[i, j];

                if (direction == 'U')
                {
                    int col = index + 3;

                    newP[8, col] = p[0, col];
                    for (int i = 0; i < 8; i++)
                    {
                        newP[i, col] = p[i + 1, col];
                    }
                }
                else if (direction == 'D')
                {
                    int col = index + 3;

                    newP[0, col] = p[8, col];
                    for (int i = 1; i < 9; i++)
                    {
                        newP[i, col] = p[i - 1, col];
                    }
                }
                else if (direction == 'L')
                {
                    int row = index + 3;

                    newP[row, 8] = p[row, 0];
                    for (int i = 0; i < 8; i++)
                    {
                        newP[row, i] = p[row, i + 1];
                    }
                }
                else if (direction == 'R')
                {
                    int row = index + 3;

                    newP[row, 0] = p[row, 8];
                    for (int i = 1; i < 9; i++)
                    {
                        newP[row, i] = p[row, i - 1];
                    }
                }

                return new LookupMove(newP, index + "" + direction,level+1);
            }

            public void Print()
            {
                for (int i = 3; i < 6; i++)
                {
                    for (int j = 3; j < 6; j++)
                    {
                        Console.Write(p[i, j] + ",");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
