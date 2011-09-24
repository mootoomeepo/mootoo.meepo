using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Collections;
using System.Data.Common;
using AutoSlidingTile;
using System.Threading;
using System.IO;

namespace ColorBlock
{
    public class AvgPathColorBlockSolver
    {
        private static SQLiteConnection conn;
        private static Random rand = new Random(1);
        public static string[] Solve(int[,] initial, ref int countStep, int maxStep, long durationPerClickMs, long timeLimitMs)
        { 

#if TRIAL
            maxStep = 25;
#endif

            InitializeDatabase();


            int[] count = new int[maxStep + 1];
            AvgPathMove bestAnswer = null;

            BinaryHeap[] nextQueue = new BinaryHeap[6];
            for (int color = 1; color < 6; color++)
                nextQueue[color] = new BinaryHeap();

            Hashtable used = new Hashtable();

            for (int color = 1; color < 6; color++)
            {
                int[,] initialColor = new int[9, 9];
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        initialColor[i, j] = (initial[i, j] == color) ? 1 : 0;

                AvgPathMove firstMove = new AvgPathMove(initialColor, null, null);
                nextQueue[color].Insert(firstMove.score, firstMove);
                used[firstMove.pos] = true;

                if (FoundSolution(firstMove, maxStep))
                {
                    bestAnswer = firstMove;
                    break;
                }
            }


            long startTime = System.DateTime.Now.Ticks;
            while (bestAnswer == null)
            {
                if (((System.DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond) > timeLimitMs)
                    return null;

                bool isBreak = true;
                for (int color = 1; color < 6; color++)
                    if (nextQueue[color].Count > 0) isBreak = false;

                if (isBreak == true) break;

                countStep++;

                bool end = false;
                for (int color = 1; color < 6; color++)
                {
                    AvgPathMove m = (AvgPathMove)(nextQueue[color].Remove());

                    AvgPathMove[] newMoves = new AvgPathMove[] {
                        m.next(0,'U'),m.next(1,'U'), m.next(2,'U'),
                        m.next(0,'D'),m.next(1,'D'), m.next(2,'D'),
                        m.next(0,'R'),m.next(1,'R'), m.next(2,'R'),
                        m.next(0,'L'),m.next(1,'L'), m.next(2,'L')
                    };


                    for (int i = 0; i < newMoves.Length; i++)
                    {
                        if (used[newMoves[i].pos] != null) continue;

                        if (FoundSolution(newMoves[i], maxStep))
                        {
                            bestAnswer = newMoves[i];
                            end = true;
                            break;
                        }

#if TRIAL
                        if (newMoves[i].level >= 19) continue;
#else
                        if (newMoves[i].level >= (maxStep - 6)) continue;
#endif

                        count[newMoves[i].level]++;


                        used[newMoves[i].pos] = true;

 
                        nextQueue[color].Insert(newMoves[i].score, newMoves[i]);

                    }

                    if (end == true) break;
                }

                if (end == true) break;
            }

            if (bestAnswer == null) return null;

            //refAnswer = bestAnswer;
            string[] steps = new string[bestAnswer.level];

            AvgPathMove b = bestAnswer;
            for (int i = steps.Length - 1; i >= 0; i--)
            {
                steps[i] = b.move;
                b = b.parent;
            }

            string[] contSteps = GetSolution(bestAnswer);

            string[] final = new string[steps.Length + contSteps.Length];
            for (int i = 0; i < steps.Length; i++)
                final[i] = steps[i];

            for (int i = 0; i < contSteps.Length; i++)
                final[i + steps.Length] = contSteps[i];

            conn.Close();

            return final;
        }

        private static string[] GetSolution(AvgPathMove bestPathMove)
        {
            List<string> buffer = new List<string>();

            while (true)
            {
                if (bestPathMove.IsFinish()) break;

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT move FROM block_colors WHERE pos='" + bestPathMove.pos + "'";

                string move = (string)cmd.ExecuteScalar();

                if (move == "") break;

                char[] c = move.ToCharArray();

                if (c[1] == 'U') c[1] = 'D';
                else if (c[1] == 'D') c[1] = 'U';
                else if (c[1] == 'L') c[1] = 'R';
                else if (c[1] == 'R') c[1] = 'L';

                string reverseMove = c[0] + "" + c[1];

                buffer.Add(reverseMove);
                cmd.Dispose();

                bestPathMove = bestPathMove.next(int.Parse(c[0] + ""), c[1]);
            }

            return buffer.ToArray();
        }

        private static bool FoundSolution(AvgPathMove bestPathMove, int maxStep)
        {

            DbCommand cmd = conn.CreateCommand();

#if TRIAL
            cmd.CommandText = "SELECT COUNT(1) FROM block_colors WHERE pos='" + bestPathMove.pos + "' AND num_steps = " + (25 - bestPathMove.level);
#else
            cmd.CommandText = "SELECT COUNT(1) FROM block_colors WHERE pos='" + bestPathMove.pos + "' AND num_steps <= " + (maxStep - bestPathMove.level);
#endif

            long num = (long)cmd.ExecuteScalar();
            cmd.Dispose();

            if (num > 0) return true;
            else return false;
        }

        private static void InitializeDatabase()
        {
            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            currentDir = currentDir.Replace("file:\\", "");

            string fullFilePath = currentDir + "/key.license";
            if (!File.Exists(fullFilePath))
            {
                throw new FileNotFoundException("License (key.license) file cannot be found");
            }

            string connString = "Data Source=" + fullFilePath+";Pooling=True;Password=mootoo-meepo-wergweguowh2rg134243terbdssdff";
            conn = new SQLiteConnection(connString);
            conn.Open();

            //DbCommand cmd = conn.CreateCommand();
            //cmd.CommandText = "CREATE TABLE IF NOT EXISTS \"block_colors\" (\"pos\" char(45) PRIMARY KEY,\"move\" char(2) NOT NULL, \"num_steps\" INTEGER NOT NULL, \"checked\" INTEGER NOT NULL);";
            //cmd.ExecuteNonQuery();
            //cmd.Dispose();

        }


    }

    public class AvgPathMove
    {
        public int[,] p;
        public string move;
        public int level;

        public string pos;
        public AvgPathMove parent;
        public int score;

        public AvgPathMove(int[,] p, string move, AvgPathMove parent)
        {
            if (parent == null) level = 0;
            else level = parent.level + 1;

            this.parent = parent;
            this.p = p;
            this.move = move;
            pos = GenPos();

            score = GetScore(p);
        }

        private static int[,] coef = new int[,] {
                        {0,0,0,1,1,1,0,0,0},
                        {0,0,0,1,1,1,0,0,0},
                        {0,0,0,3,3,3,0,0,0},
                        {1,1,3,6,7,6,3,1,1},
                        {1,1,3,7,9,7,3,1,1},
                        {1,1,3,6,7,6,3,1,1},
                        {0,0,0,3,3,3,0,0,0},
                        {0,0,0,1,1,1,0,0,0},
                        {0,0,0,1,1,1,0,0,0}
        };

        private static int GetScore(int[,] p)
        {
            int score = 0;


            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (p[i,j] > 0)
                        score += coef[i,j];
                }
            }


            return score;
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


        public AvgPathMove next(int index, char direction)
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

            return new AvgPathMove(newP, index + "" + direction, this);
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

        internal bool IsFinish()
        {
            for (int i = 3; i < 6; i++)
            {
                for (int j = 3; j < 6; j++)
                {
                    if (p[i, j] != 1) return false;
                }
            }

            return true;
        }
    }
}
