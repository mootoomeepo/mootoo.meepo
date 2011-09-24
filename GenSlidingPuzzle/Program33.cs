using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using System.Threading;

namespace GenSlidingPuzzle
{
    class Program
    {
        public static string tableName = "tiles33";
        public static long count;
        public static int lastLevel;
        public static bool isRunning = false;
        static void Main(string[] args)
        {
            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            currentDir = currentDir.Replace("file:\\", "");

            string connString = "Data Source=" + currentDir + "/parity_check.license;Pooling=True;Password=mootoo-meepo-wergweguowh2rg134243terbdssdff";
            SQLiteConnection conn = new SQLiteConnection(connString);
            conn.Open();

            int[] p = new int[9] {
                0,1,2,3,4,5,6,7,8
            };
            Move perfect = new Move(p,9, -1,0);

            string createTableSql = "CREATE TABLE IF NOT EXISTS \"" + tableName + "\" (\"pos\" char(" + perfect.signature.Length + ") PRIMARY KEY,\"before_empty\" INTEGER NOT NULL, \"num_steps\" INTEGER NOT NULL, \"checked\" INTEGER NOT NULL);";

            ExecuteDbCommand(createTableSql, conn);


            try
            {
                ExecuteDbCommand("INSERT INTO "+tableName+" (pos,before_empty,num_steps,checked) VALUES ('" + perfect.signature + "','" + perfect.emptyBefore + "','" + perfect.level + "','0');", conn);
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
                Move m = GetNext(conn);

                if (m == null) break;

                Move[] newMoves = new Move[] {
                        m.UpMove(),m.RightMove(),m.LeftMove(),m.DownMove()
                    };

                for (int i = 0; i < newMoves.Length; i++)
                {
                    if (newMoves[i] == null) continue;
                    try
                    {
                        ExecuteDbCommand("INSERT INTO " + tableName + " (pos,before_empty,num_steps,checked) VALUES ('" + newMoves[i].signature + "','" + newMoves[i].emptyBefore + "','" + newMoves[i].level + "','0');", conn);

                        count++;
                        lastLevel = newMoves[i].level;

                        //if (newMoves[i].level < 25) q.Enqueue(newMoves[i]);
                    }
                    catch (SQLiteException e)
                    {
                        
                    }
                }

                ExecuteDbCommand("UPDATE " + tableName + " SET checked='1' WHERE pos='" + m.signature + "'", conn);

            }

            isRunning = false;

            Console.WriteLine("DONE");
            Console.ReadLine();
        }

        private static long GetNum(SQLiteConnection conn)
        {
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(1) FROM "+tableName;

            long count = (long)(cmd.ExecuteScalar());

            cmd.Dispose();

            return count;
        }

        private static Move GetNext(SQLiteConnection conn)
        {
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM " + tableName + " WHERE checked='0' ORDER BY num_steps ASC LIMIT 1";

            DbDataReader reader = cmd.ExecuteReader();

            Move m = null;
            if (reader.Read())
            {
                string pos = (string)reader["pos"];

                string[] tokens = pos.Split(',');

                int[] p = new int[9];
                int emptyPosition = -1;
                for (int i = 0; i < p.Length; i++)
                {
                    p[i] = int.Parse(tokens[i]);

                    if (p[i] == (p.Length-1)) emptyPosition = i;
                }

                m = new Move(p,emptyPosition, (int)((long)(reader["before_empty"])), (int)((long)(reader["num_steps"])));
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

        public class Move
        {
            public int emptyPosition = -1;
            public int[] current;
            public int emptyBefore;
            public int level;
            public string signature;

            public static int dx = 3;
            public static int dy = 3;
            public Move(int[] current,int emptyPosition, int emptyBefore,int level)
            {
                this.level = level;
                this.current = current;
                this.emptyBefore = emptyBefore;

                this.emptyPosition = emptyPosition;

                this.signature = GetSignature();
            }

            private string GetSignature()
            {
                StringBuilder s = new StringBuilder();

                for (int i = 0; i < current.Length; i++)
                {
                    s.Append(current[i] + ","); 
                }

                return s.ToString();
            }

            public override string ToString()
            {
                StringBuilder s = new StringBuilder();

                for (int i = 0; i < current.Length; i++)
                {
                    if ((i % dx) == 0) s.Append("\r\n");
                    s.Append(current[i] + ",");

                }

                s.Append("\r\n(");


                return s.ToString();
            }

            private Move Next(int clickPos)
            {
                int[] new_cur = new int[current.Length];
                for (int i = 0; i < new_cur.Length; i++) new_cur[i] = current[i];

                //swap
                int temp = new_cur[emptyPosition];
                new_cur[emptyPosition] = new_cur[clickPos];
                new_cur[clickPos] = temp;

                return new Move(new_cur, clickPos, emptyPosition, level + 1);
            }

            internal Move DownMove()
            {
                if ((emptyPosition + dy) >= current.Length) return null;
                return Next(emptyPosition + dy);
            }

            internal Move UpMove()
            {
                if ((emptyPosition - dy) < 0) return null;
                return Next(emptyPosition - dy);
            }

            internal Move RightMove()
            {
                if ((emptyPosition % dx) == (dx-1)) return null;
                return Next(emptyPosition +1);
            }

            internal Move LeftMove()
            {
                if ((emptyPosition % dx) == 0) return null;
                return Next(emptyPosition - 1);
            }
        }
    }
}
