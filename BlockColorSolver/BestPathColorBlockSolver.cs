using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SQLite;
using System.Data.Common;

namespace ColorBlock
{
    public class BestPathColorBlockSolver
    {
        private static SQLiteConnection conn;
        public static string[] Solve(int[,] initial, ref int countStep, ref BestPathMove refAnswer, int maxStep, long durationPerClickMs, long timeLimitMs)
        {
            InitializeDatabase();

            long startTime = DateTime.Now.Ticks;

            int[] count = new int[maxStep + 1];
            BestPathMove bestAnswer = null;

            Queue<BestPathMove>[] nextQueue = new Queue<BestPathMove>[6];
            for (int color = 1; color < 6; color++)
                nextQueue[color] = new Queue<BestPathMove>();

            Hashtable used = new Hashtable();

            for (int color=1;color<6;color++)
            {
                int[,] initialColor = new int[9, 9];
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        initialColor[i, j] = (initial[i, j] == color) ? 1 : 0;

                BestPathMove firstMove = new BestPathMove(initialColor, null, null);
                nextQueue[color].Enqueue(firstMove);
                used[firstMove.pos] = true;

                if (FoundSolution(firstMove))
                {
                    bestAnswer = firstMove;
                    break;
                }
            }

            

            while (bestAnswer == null)
            {
                bool isBreak = true;
                for (int color = 1; color < 6; color++)
                    if (nextQueue[color].Count > 0) isBreak = false;

                if (isBreak == true) break;

                countStep++;

                bool end = false;
                for (int color = 1; color < 6; color++)
                {
                    BestPathMove m = nextQueue[color].Dequeue();

                    BestPathMove[] newMoves = new BestPathMove[] {
                        m.next(0,'U'),m.next(1,'U'), m.next(2,'U'),
                        m.next(0,'D'),m.next(1,'D'), m.next(2,'D'),
                        m.next(0,'R'),m.next(1,'R'), m.next(2,'R'),
                        m.next(0,'L'),m.next(1,'L'), m.next(2,'L')
                    };

                    
                    for (int i = 0; i < newMoves.Length; i++)
                    {
                        if (used[newMoves[i].pos] != null) continue;

                        if (FoundSolution(newMoves[i]))
                        {
                            bestAnswer = newMoves[i];
                            end = true;
                            break;
                        }

                        count[newMoves[i].level]++;
                        

                        used[newMoves[i].pos] = true;

                        if (newMoves[i].level >= maxStep) continue;
                        nextQueue[color].Enqueue(newMoves[i]);

                    }
                    
                    if (end == true) break;
                }

                if (end == true) break;
            }

            refAnswer = bestAnswer;
            string[] steps = new string[bestAnswer.level];

            BestPathMove b = bestAnswer;
            for (int i = steps.Length-1; i >= 0; i--)
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

            return final;
        }

        private static string[] GetSolution(BestPathMove bestPathMove)
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

                string reverseMove = c[0]+""+c[1];

                buffer.Add(reverseMove);
                cmd.Dispose();

                bestPathMove = bestPathMove.next(int.Parse(c[0] + ""), c[1]);
            }

            return buffer.ToArray();
        }

        private static bool FoundSolution(BestPathMove bestPathMove)
        {

            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(1) FROM block_colors WHERE pos='"+bestPathMove.pos+"'";

            long num = (long)cmd.ExecuteScalar();
            cmd.Dispose();

            if (num > 0) return true;
            else return false;
        }

        private static void InitializeDatabase()
        {
            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            currentDir = currentDir.Replace("file:\\", "");

            string connString = "Data Source=" + currentDir + "/pattern.sqlite;Pooling=True";
            conn = new SQLiteConnection(connString);
            conn.Open();
        }


    }

    public class BestPathMove
    {
        public int[,] p;
        public string move;
        public int level;

        public string pos;
        public BestPathMove parent;

        public BestPathMove(int[,] p, string move, BestPathMove parent)
        {
            if (parent == null) level = 0;
            else level = parent.level + 1;

            this.parent = parent;
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


        public BestPathMove next(int index, char direction)
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

            return new BestPathMove(newP, index + "" + direction, this);
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
