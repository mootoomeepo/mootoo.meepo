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

            string createTableSql = "CREATE TABLE IF NOT EXISTS \"block_colors\" (\"pos\" char(45) PRIMARY KEY,\"move\" char(2) NOT NULL, \"num_steps\" INTEGER NOT NULL, \"checked\" INTEGER NOT NULL);";

            ExecuteDbCommand(createTableSql, conn);

            //ExecuteDbCommand("DELETE FROM block_colors;", conn);

            int[,] p = new int[9, 9] {
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,1,1,1,0,0,0},
                {0,0,0,1,1,1,0,0,0},
                {0,0,0,1,1,1,0,0,0},
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0}
            };

            //q = new Queue<LookupMove>();

            LookupMove perfect = new LookupMove(p, "", 0);

            try
            {
                ExecuteDbCommand("INSERT INTO block_colors (pos,move,num_steps,checked) VALUES ('" + perfect.pos + "','" + perfect.move + "','" + perfect.level + "','0');", conn);
                //q.Enqueue(perfect);
            }
            catch (SQLiteException e)
            {
                
            }

            isRunning = true;
            OutputClass o = new OutputClass();
            new Thread(new ThreadStart(o.output)).Start();

            count = GetNum(conn);
            while (true)
            {
                //if (q.Count == 0) break;

                //LookupMove m = q.Dequeue();
                LookupMove m = GetNext(conn);

                if (m == null) break;

                LookupMove[] newMoves = new LookupMove[] {
                        m.next(0,'U'),m.next(1,'U'), m.next(2,'U'),
                        m.next(0,'D'),m.next(1,'D'), m.next(2,'D'),
                        m.next(0,'R'),m.next(1,'R'), m.next(2,'R'),
                        m.next(0,'L'),m.next(1,'L'), m.next(2,'L')
                    };

                for (int i = 0; i < newMoves.Length; i++)
                {
                    try
                    {
                        ExecuteDbCommand("INSERT INTO block_colors (pos,move,num_steps,checked) VALUES ('" + newMoves[i].pos + "','" + newMoves[i].move + "','" + newMoves[i].level + "','0');", conn);

                        count++;
                        lastLevel = newMoves[i].level;

                        //if (newMoves[i].level < 25) q.Enqueue(newMoves[i]);
                    }
                    catch (SQLiteException e)
                    {
                        
                    }
                }

                ExecuteDbCommand("UPDATE block_colors SET checked='1' WHERE pos='" + m.pos + "'", conn);

            }

            isRunning = false;

            Console.WriteLine("DONE");
            Console.ReadLine();
        }

        private static long GetNum(SQLiteConnection conn)
        {
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(1) FROM block_colors";

            long count = (long)(cmd.ExecuteScalar());

            cmd.Dispose();

            return count;
        }

        private static LookupMove GetNext(SQLiteConnection conn)
        {
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM block_colors WHERE checked='0' ORDER BY num_steps ASC LIMIT 1";

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
