using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading;

using System.Windows.Forms;

namespace RotationPuzzleSolver
{
    class BuildDb
    {
        public static int dimension = 4;

        public static string tableName = "rotation";
        public static long count;
        public static int lastLevel;
        public static bool isRunning = false;
        static void Main(string[] args)
        {
            tableName += "" + dimension;

            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            currentDir = currentDir.Replace("file:\\", "");

            string connString = "Data Source=" + currentDir + "/key_vault.license;Pooling=True";//;Password=mootoo-meepo-wergweguowh2rg134243terbdssdff";
            SQLiteConnection conn = new SQLiteConnection(connString);
            conn.Open();

            int[] p = new int[dimension * dimension];
            for (int i = 0; i < p.Length; i++) p[i] = i;


            DbMove perfect = new DbMove(p, "XXX", 0, dimension, dimension);

            string createTableSql = "CREATE TABLE IF NOT EXISTS \"" + tableName + "\" (\"pos\" char(100) PRIMARY KEY,\"move\" char(3) NOT NULL, \"num_steps\" INTEGER NOT NULL, \"checked\" INTEGER NOT NULL);";

            ExecuteDbCommand(createTableSql, conn);


            try
            {
                ExecuteDbCommand("INSERT INTO " + tableName + " (pos,move,num_steps,checked) VALUES ('" + perfect.signature + "','" + perfect.move + "','" + perfect.level + "','0');", conn);
                //q.Enqueue(perfect);
            }
            catch (SQLiteException e)
            {

            }

            isRunning = true;
            OutputClass o = new OutputClass();
            new Thread(new ThreadStart(o.output)).Start();


            int clickD = dimension - 1;
            int clickD2 = clickD * clickD;

            count = GetNum(conn);
            while (true)
            {
                DbMove m = GetNext(conn);

                if (m == null) break;

                DbMove[] newMoves = new DbMove[clickD2 * 2];

                for (int k = 0; k < newMoves.Length; k++)
                {
                    if ((k % 2) == 0)
                        newMoves[k] = m.GenMove(k / 2, DbMove.Direction.CLOCKWISE);
                    else
                        newMoves[k] = m.GenMove(k / 2, DbMove.Direction.COUNTER_CLOCKWISE);
                }

                for (int i = 0; i < newMoves.Length; i++)
                {
                    if (newMoves[i] == null) continue;
                    try
                    {
                        ExecuteDbCommand("INSERT INTO " + tableName + " (pos,move,num_steps,checked) VALUES ('" + newMoves[i].signature + "','" + newMoves[i].move + "','" + newMoves[i].level + "','0');", conn);

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
            cmd.CommandText = "SELECT COUNT(1) FROM " + tableName;

            long count = (long)(cmd.ExecuteScalar());

            cmd.Dispose();

            return count;
        }

        private static DbMove GetNext(SQLiteConnection conn)
        {
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM " + tableName + " WHERE checked='0' ORDER BY num_steps ASC LIMIT 1";

            DbDataReader reader = cmd.ExecuteReader();

            DbMove m = null;
            if (reader.Read())
            {
                string pos = (string)reader["pos"];

                string[] tokens = pos.Split(',');

                int[] p = new int[dimension*dimension];
                int emptyPosition = -1;
                for (int i = 0; i < p.Length; i++)
                {
                    p[i] = int.Parse(tokens[i]);

                    if (p[i] == (p.Length - 1)) emptyPosition = i;
                }

                m = new DbMove(p, (string)(reader["move"]), (int)((long)(reader["num_steps"])), dimension, dimension);
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
                    Console.WriteLine("dimension=" + dimension+" db=" + BuildDb.count + " level=" + BuildDb.lastLevel);
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


        public class DbMove
        {

            public int[] current;
            public int level;

            private int dx;
            private int dy;

            public bool IsFinished = false;
            public string signature;

            public string move;

            public DbMove(int[] current, string move, int level, int dx, int dy)
            {
                this.level = level;

                this.dx = dx;
                this.dy = dy;
                this.current = current;
                this.move = move;


                IsFinished = true;
                for (int i = 0; i < current.Length; i++)
                {
                    if (current[i] != i)
                    {
                        IsFinished = false;
                    }
                }

                signature = GetSignature();
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

            public enum Direction { CLOCKWISE, COUNTER_CLOCKWISE };

            public DbMove GenMove(int p, Direction d)
            {
                // make a move
                int[] new_current = new int[current.Length];

                for (int i = 0; i < new_current.Length; i++)
                    new_current[i] = current[i];


                int row = p / (dx - 1);
                int col = p % (dx - 1);

                int[] miniPos = new int[4] {    row * dx + col, 
                                                row*dx + col + 1,
                                                (row+1)*dx + col+1,
                                                (row+1)*dx + col
                                                
                                            };

                if (d == Direction.CLOCKWISE)
                {
                    for (int i = 0; i < miniPos.Length; i++)
                    {
                        int next = i - 1;
                        if (next == -1) next = miniPos.Length-1;
                        new_current[miniPos[i]] = current[miniPos[next]];
                    }
                }
                else
                {
                    for (int i = 0; i < miniPos.Length; i++)
                        new_current[miniPos[i]] = current[miniPos[(i + 1) % 4]];
                }

                // gen move signature
                string moveSignature = addZero(p);

                if (d == Direction.CLOCKWISE) moveSignature += "C";
                else if (d == Direction.COUNTER_CLOCKWISE) moveSignature += "T";

                return new DbMove(new_current, moveSignature, level+1, dx, dy);
            }

            private string addZero(int p)
            {
                if (p < 10) return "0" + p;
                else return "" + p;
            }

        }
    }
}
