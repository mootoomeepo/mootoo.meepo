using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;
using AutoSlidingTile;

namespace RotationPuzzle
{
    public class RotationPuzzleSolver
    {

        private static SQLiteConnection conn;

        public static string[] Process(int dx,int dy,int[] pieces, ref int countStep)
        {
            return Calculate(dx,dy,pieces,ref countStep);
        }

        private static string[] Calculate(int dx, int dy, int[] pieces, ref int countStep)
        {
            InitializeDatabase();

            BinaryHeap nextQueue = new BinaryHeap();
            Hashtable used = new Hashtable();

            Move finish = null;

            Move firstMove = new Move(pieces, "", null, dx, dy);
            nextQueue.Insert(firstMove.score, firstMove);

            int clickD = dx - 1;
            int clickD2 = clickD * clickD;

            used[firstMove.signature] = true;

            while (true)
            {
                if (nextQueue.Count == 0) break;

                countStep++;
                Move m = (Move)(nextQueue.Remove());
                if (m.IsFinished || FoundSolution(m,dx))
                {
                    finish = m;
                    break;
                }

                Move[] newMoves = new Move[clickD2*2];
                
                for (int k=0;k<newMoves.Length;k++)
                {
                    if ((k%2) == 0)
                        newMoves[k] = m.GenMove(k/2,Move.Direction.CLOCKWISE);
                    else
                        newMoves[k] = m.GenMove(k/2,Move.Direction.COUNTER_CLOCKWISE);
                }

                for (int i = 0; i < newMoves.Length; i++)
                {
                    if (newMoves[i] == null) continue;

                    if (newMoves[i].IsFinished || FoundSolution(m,dx))
                    {
                        finish = newMoves[i];
                        break;
                    }

                    if (used[newMoves[i].signature] != null) continue;

                    used[newMoves[i].signature] = true;

                    nextQueue.Insert(newMoves[i].score,newMoves[i]);
                    //Console.WriteLine(newMoves[i].ToString());
                    //Console.ReadKey();
                }

                if (finish != null) break;
            }

            if (finish == null) return null;

            if (!finish.IsFinished)
            {
                finish = GetSolution(finish,dx);
            }

            List<string> steps = new List<string>();

            while (finish != null)
            {
                steps.Insert(0, finish.move);
                finish = finish.parent;
            }

            steps.RemoveAt(0);

            return steps.ToArray();
        }


        private static void InitializeDatabase()
        {
            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            currentDir = currentDir.Replace("file:\\", "");

            string fullFilePath = currentDir + "/key_vault.license";
            if (!File.Exists(fullFilePath))
            {
                throw new FileNotFoundException("License (key_vault.license) file cannot be found");
            }

            string connString = "Data Source=" + fullFilePath + ";Pooling=True";//;Password=mootoo-meepo-wergweguowh2rg134243terbdssdff";
            conn = new SQLiteConnection(connString);
            conn.Open();
        }

        private static Move GetSolution(Move m,int dimension)
        {
            Move finishMove = m;

            int count = 0;
            while (true)
            {
                if (finishMove.IsFinished) break;

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT move FROM rotation"+dimension+" WHERE pos='" + finishMove.signature + "'";

                string move = ((string)cmd.ExecuteScalar());

                string posString = move.Substring(0, 2);
                string directionString = move.Substring(2, 1);


                int p = int.Parse(posString);
                

                // reverse it
                Move.Direction d = Move.Direction.CLOCKWISE;

                if (directionString == "T") d = Move.Direction.CLOCKWISE;
                else if (directionString == "C") d = Move.Direction.COUNTER_CLOCKWISE;
                else throw new Exception("Invalid Direction");

                finishMove = finishMove.GenMove(p, d);

                cmd.Dispose();
                count++;
            }

            //Console.WriteLine("Save = " + count + " steps");

            return finishMove;
        }

        private static bool FoundSolution(Move m,int dimension)
        {
            DbCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT COUNT(1) FROM rotation" + dimension + " WHERE pos='" + m.signature + "'";
            long num = (long)cmd.ExecuteScalar();
            cmd.Dispose();

            if (num > 0) return true;
            else return false;
        }




        public class Move
        {

            public int[] current;
            public Move parent;
            public int level;

            private int dx;
            private int dy;

            public bool IsFinished = false;
            public string signature;
            public int score;

            public string move;

            public Move(int[] current, string move, Move parent, int dx, int dy)
            {
                if (parent == null) level = 0;
                else level = parent.level + 1;

                this.dx = dx;
                this.dy = dy;
                this.current = current;
                this.parent = parent;
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
                score = GetScore();
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

            public enum Direction {CLOCKWISE,COUNTER_CLOCKWISE}


            public Move GenMove(int p, Direction d)
            {
                // make a move
                int[] new_current = new int[current.Length];

                for (int i=0;i<new_current.Length;i++)
                    new_current[i] = current[i];


                int row = p / (dx-1);
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
                        if (next == -1) next = 3;
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

                return new Move(new_current, moveSignature, this, dx, dy);
            }

            private string addZero(int p)
            {
                if (p < 10) return "0" + p;
                else return "" + p;
            }

            private int GetScore()
            {
                if (dx == 3) return level * -1;

                int sum = 0;
                for (int i = 0; i < current.Length; i++)
                {
                    int x = i % dx;
                    int y = i / dx;

                    int ox = current[i] % dx;
                    int oy = current[i] / dx;

                    int rx = Math.Abs(ox - x);
                    int ry = Math.Abs(oy - y);
                    sum -= (rx + ry);
                }

                return sum - level;
            }
        }
    }
}
