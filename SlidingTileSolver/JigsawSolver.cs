using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace AutoSlidingTile
{
    class JigsawSolver
    {
        private static SQLiteConnection conn;

        public static int[] Process(int dimension, int[] pieces, ref int countStep)
        {
            InitializeDatabase();

            // check for validity
            int sum = 0;

            for (int i = 0; i < pieces.Length; i++)
            {
                int count = 0;

                for (int j = 0; j < pieces.Length; j++)
                {
                    if (pieces[j] == i) count++;
                }
                if (count != 1) throw new InvalidCombinationException(dimension);
            }

            if (!IsValid(pieces)) throw new InvalidCombinationException(dimension);

#if TRIAL
            return Calculate(3,3,pieces,ref countStep);
#else
            return (dimension == 4) ? Calculate44(pieces, ref countStep) : Calculate(3, 3, pieces, ref countStep);
#endif
        }

#if TRIAL

#else
        private static int FACTOR = 3;
        private static Move Calculate44Border(int[] pieces, ref int countStep)
        {
            

            Move borderMove = null;
            BinaryHeap nextQueue = new BinaryHeap();
            Hashtable used = new Hashtable();

            Move firstMove = new Move(pieces, -1, null, 4, 4);
            nextQueue.Insert(firstMove.borderScore - firstMove.level / FACTOR, firstMove);

            used[firstMove.borderSignature] = true;

            while (true)
            {
                if (nextQueue.Count == 0) break;

                countStep++;
                //Console.WriteLine(countStep);

                Move m = (Move)(nextQueue.Remove());
                if (m.IsBorderFinished || FoundBorderSolution(m))
                {
                    borderMove = m;
                    break;
                }

                Move[] newMoves = new Move[] { m.DownMove(), m.UpMove(), m.RightMove(), m.LeftMove() };

                for (int i = 0; i < newMoves.Length; i++)
                {
                    if (newMoves[i] == null) continue;

                    
                    if (newMoves[i].IsBorderFinished || FoundBorderSolution(newMoves[i]))
                    {
                        borderMove = newMoves[i];
                        break;
                    }

                    if (used[newMoves[i].borderSignature] != null) continue;

                    //if (m.borderScore > (newMoves[i].borderScore+1)) continue;

                    used[newMoves[i].borderSignature] = true;
                    nextQueue.Insert(newMoves[i].borderScore - newMoves[i].level / FACTOR, newMoves[i]);
                    //Console.WriteLine(newMoves[i].ToBorderString());
                    //Console.WriteLine(newMoves[i].borderScore);

                    //if (newMoves[i].IsInvalid)
                        //Console.ReadKey();
                }

                if (borderMove != null) break;
            }

            if (!borderMove.IsBorderFinished)
            {
                borderMove = GetBorderSolution(borderMove);
            }

            //Console.WriteLine("4x4 border = " + countStep);
            conn.Clone();

            return borderMove;
        }

        private static Move GetBorderSolution(Move borderMove)
        {
            Move finishMove = borderMove;

            int count = 0;
            while (true)
            {
                if (finishMove.IsBorderFinished) break;

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT before_empty FROM border_tiles WHERE pos='" + finishMove.borderSignature + "'";

                int click = (int)((long)cmd.ExecuteScalar());

                if (click == (finishMove.emptyPosition + 1))
                    finishMove = finishMove.RightMove();
                else if (click == (finishMove.emptyPosition - 1))
                    finishMove = finishMove.LeftMove();
                else if (click == (finishMove.emptyPosition + 4))
                    finishMove = finishMove.DownMove();
                else if (click == (finishMove.emptyPosition - 4))
                    finishMove = finishMove.UpMove();

                cmd.Dispose();
                count++;
            }

            //Console.WriteLine("Save = " + count + " steps");

            return finishMove;
        }


        private static bool FoundBorderSolution(Move m)
        {
            DbCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT COUNT(1) FROM border_tiles WHERE pos='" + m.borderSignature + "'";
            long num = (long)cmd.ExecuteScalar();
            cmd.Dispose();

            if (num > 0) return true;
            else return false;
        }


        private static int[] Calculate44(int[] pieces, ref int count)
        {
            int d = 4;
            // Do borders
            //  0, 1,  2,  3
            //  4, x,  x,  x
            //  8, x,  x,  x
            //  12, x,  x,  x

            Move borderMove = Calculate44Border(pieces, ref count);

            List<int> bufferSteps = new List<int>();

            Move finishBorder = borderMove;
            while (finishBorder != null)
            {
                bufferSteps.Insert(0, finishBorder.emptyPosition);
                finishBorder = finishBorder.parent;
            }

            bufferSteps.RemoveAt(0);

            int[] steps = bufferSteps.ToArray();

            int[] pieces33 = TransformTo33(borderMove.current);
            int[] steps33 = Process(3, pieces33, ref count);

            int[] finalSteps = new int[steps.Length + steps33.Length];

            for (int i = 0; i < steps.Length; i++) finalSteps[i] = steps[i];

            int N = steps.Length;

            for (int i = 0; i < steps33.Length; i++)
            {
                if (0 <= steps33[i] && steps33[i] <= 2)
                    finalSteps[i + N] = steps33[i] + 5;
                else if (3 <= steps33[i] && steps33[i] <= 5)
                    finalSteps[i + N] = steps33[i] + 6;
                else if (6 <= steps33[i] && steps33[i] <= 8)
                    finalSteps[i + N] = steps33[i] + 7;
            }

            return finalSteps;
        }
#endif

        private static void InitializeDatabase()
        {
            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            currentDir = currentDir.Replace("file:\\", "");

            string fullFilePath = currentDir + "/parity_check.license";
            if (!File.Exists(fullFilePath))
            {
                throw new FileNotFoundException("License (parity_check.license) file cannot be found");
            }

            string connString = "Data Source=" + fullFilePath + ";Pooling=True;Password=mootoo-meepo-wergweguowh2rg134243terbdssdff";
            conn = new SQLiteConnection(connString);
            conn.Open();

            //int[] p = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            //Move m = new Move(p, 0, null, 3, 3);

            //{
            //    DbCommand cmd = conn.CreateCommand();
            //    cmd.CommandText = "CREATE TABLE IF NOT EXISTS \"border_tiles\" (\"pos\" char(" + m.signature.Length + ") PRIMARY KEY,\"before_empty\" INTEGER NOT NULL, \"num_steps\" INTEGER NOT NULL, \"checked\" INTEGER NOT NULL);";
            //    cmd.ExecuteNonQuery();
            //    cmd.Dispose();
            //}
            //{
            //    DbCommand cmd = conn.CreateCommand();
            //    cmd.CommandText = "CREATE TABLE IF NOT EXISTS \"tiles33\" (\"pos\" char(" + m.signature.Length + ") PRIMARY KEY,\"before_empty\" INTEGER NOT NULL, \"num_steps\" INTEGER NOT NULL, \"checked\" INTEGER NOT NULL);";
            //    cmd.ExecuteNonQuery();
            //    cmd.Dispose();
            //}
        }

        private static bool IsValid(int[] pieces)
        {
            int d = (pieces.Length == 16) ? 4 : 3;

            int emptyPos = -1;

            // check for validity
            int sum = 0;
            for (int i = 0; i < pieces.Length; i++)
            {
                if (pieces[i] == (pieces.Length - 1))
                {
                    emptyPos = i;
                }
                else
                {
                    for (int j = i + 1; j < pieces.Length; j++)
                    {
                        if (pieces[i] > pieces[j])
                            sum++;
                    }
                }

            }

            if (d == 3)
            {
                return ((sum % 2) == 0);
            }
            else
            {
                if (((emptyPos/4)%2)==0)
                    return ((sum % 2) == 1);
                else
                    return ((sum % 2) == 0);
            }
        }

        private static Move Get33Solution(Move borderMove)
        {
            Move finishMove = borderMove;

            int count = 0;
            while (true)
            {
                if (finishMove.IsBorderFinished) break;

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT before_empty FROM tiles33 WHERE pos='" + finishMove.signature + "'";

                int click = (int)((long)cmd.ExecuteScalar());

                if (click == (finishMove.emptyPosition + 1))
                    finishMove = finishMove.RightMove();
                else if (click == (finishMove.emptyPosition - 1))
                    finishMove = finishMove.LeftMove();
                else if (click == (finishMove.emptyPosition + 3))
                    finishMove = finishMove.DownMove();
                else if (click == (finishMove.emptyPosition - 3))
                    finishMove = finishMove.UpMove();

                cmd.Dispose();
                count++;
            }

            //Console.WriteLine("Save = " + count + " steps");

            return finishMove;
        }


        private static bool Found33Solution(Move m)
        {
            DbCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT COUNT(1) FROM tiles33 WHERE pos='" + m.signature + "'";
            long num = (long)cmd.ExecuteScalar();
            cmd.Dispose();

            if (num > 0) return true;
            else return false;
        }



#if TRIAL

#else
        private static int[] TransformTo33(int[] pieces)
        {
            // Do 3x3
            int[] pieces33 = { pieces[5], pieces[6], pieces[7],
                           pieces[9], pieces[10], pieces[11],
                           pieces[13], pieces[14], pieces[15]
                         };

            for (int i = 0; i < pieces33.Length; i++)
            {
                if (pieces33[i] == 5) pieces33[i] = 0;
                else if (pieces33[i] == 6) pieces33[i] = 1;
                else if (pieces33[i] == 7) pieces33[i] = 2;
                else if (pieces33[i] == 9) pieces33[i] = 3;
                else if (pieces33[i] == 10) pieces33[i] = 4;
                else if (pieces33[i] == 11) pieces33[i] = 5;
                else if (pieces33[i] == 13) pieces33[i] = 6;
                else if (pieces33[i] == 14) pieces33[i] = 7;
                else if (pieces33[i] == 15) pieces33[i] = 8;
            }
            return pieces33;
        }
#endif

        private static int[] Calculate(int dx, int dy, int[] pieces, ref int countStep)
        {
            Queue<Move> nextQueue = new Queue<Move>();
            Hashtable used = new Hashtable();

            Move finish = null;

            Move firstMove = new Move(pieces, -1, null, dx, dy);
            nextQueue.Enqueue(firstMove);

            //if ((score % 2) > 0) return null;

            used[firstMove.signature] = true;

            while (true)
            {
                if (nextQueue.Count == 0) break;

                countStep++;
                Move m = nextQueue.Dequeue();
                if (m.IsFinished || Found33Solution(m))
                {
                    finish = m;
                    break;
                }

                Move[] newMoves = new Move[] {m.DownMove(),m.UpMove(), m.RightMove(),m.LeftMove()};

                for (int i = 0; i < newMoves.Length; i++)
                {
                    if (newMoves[i] == null) continue;

                    if (newMoves[i].IsFinished || Found33Solution(m))
                    {
                        finish = newMoves[i];
                        break;
                    }

                    if (used[newMoves[i].signature] != null) continue;

                    used[newMoves[i].signature] = true;

                    nextQueue.Enqueue(newMoves[i]);
                    //Console.WriteLine(newMoves[i].ToString());
                    //Console.ReadKey();
                }

                if (finish != null) break;
            }

            if (finish == null) return null;

            if (!finish.IsFinished)
            {
                finish = Get33Solution(finish);
            }

            List<int> steps = new List<int>();

            while (finish != null)
            {
                steps.Insert(0,finish.emptyPosition);
                finish = finish.parent;
            }

            steps.RemoveAt(0);

            return steps.ToArray();
        }
    }

    public class Move
    {
        public int emptyPosition = -1;
        public int[] current;
        public Move parent;
        public int level;

        private int dx;
        private int dy;

        public bool IsFinished = false;
        public bool IsBorderFinished = false;

        public string borderSignature;
        public string signature;

        public int borderScore;

        public Move(int[] current,int click, Move parent,int dx,int dy)
        {
            if (parent == null) level = 0;
            else level = parent.level + 1;

            this.dx = dx;
            this.dy = dy;
            this.current = current;
            this.parent = parent;
            this.emptyPosition = click;

            if (emptyPosition == -1)
            {
                for (int i = 0; i < current.Length; i++)
                {
                    if (current[i] == (current.Length-1))
                    {
                        emptyPosition = i;
                    }
                }
            }

            IsFinished = true;
            IsBorderFinished = true;
            for (int i = 0; i < current.Length; i++)
            {
                if (current[i] != i)
                {
                    IsFinished = false;
                }

                if ((0 <= current[i] && current[i] <= 3)
                    || current[i] == 4
                    || current[i] == 8
                    || current[i] == 12)
                {
                    if (current[i] != i)
                    {
                        IsBorderFinished = false;
                    }
                }
            }

            borderScore = GetBorderScore();
            borderSignature = GetBorderSignature();
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

        private Move GenSwap(int emptyPosition, int clickPos)
        {
            int[] new_cur = new int[current.Length];
            for (int i = 0; i < new_cur.Length; i++) new_cur[i] = current[i];

            //swap
            int temp = new_cur[emptyPosition];
            new_cur[emptyPosition] = new_cur[clickPos];
            new_cur[clickPos] = temp;

            return new Move(new_cur,clickPos, this, dx,dy);
        }

        internal Move DownMove()
        {
            if ((emptyPosition + dy) >= current.Length) return null;
            return GenSwap(emptyPosition, emptyPosition + dy);
        }

        internal Move UpMove()
        {
            if ((emptyPosition - dy) < 0) return null;
            return GenSwap(emptyPosition, emptyPosition - dy);
        }

        internal Move RightMove()
        {
            if ((emptyPosition % dx) == (dx-1)) return null;
            return GenSwap(emptyPosition, emptyPosition +1);
        }

        internal Move LeftMove()
        {
            if ((emptyPosition % dx) == 0) return null;
            return GenSwap(emptyPosition, emptyPosition - 1);
        }

        private string GetBorderSignature()
        {
            StringBuilder s = new StringBuilder();

            for (int i = 0; i < current.Length; i++)
            {
                if ((0 <= current[i] && current[i] <= 3)
                    || current[i] == 4
                    || current[i] == 8
                    || current[i] == 12
                    || current[i] == 15)
                {
                    s.Append(current[i] + ",");
                }
                else
                {
                    s.Append("-1,");
                }

            }

            return s.ToString();
        }

        public string ToBorderString()
        {
            StringBuilder s = new StringBuilder();

            for (int i = 0; i < current.Length; i++)
            {
                if ((i % dx) == 0) s.Append("\r\n");

                if ((0 <= current[i] && current[i] <= 3)
                    || current[i] == 4
                    || current[i] == 8
                    || current[i] == 12
                    || current[i] == 15)
                {
                    s.Append(current[i] + ",");
                }
                else
                {
                    s.Append("x,");
                }

            }

            s.Append("\r\n(");

            return s.ToString();
        }

 
        private int GetBorderScore()
        {
            if (current.Length != 16) return 0;


            int sum = 0;
            for (int i = 0; i < current.Length; i++)
            {
                if (current[i] == 12) ;
                else if (current[i] == 8) ;
                else if (current[i] == 4) ;
                else if (current[i] == 0) ;
                else if (current[i] == 1) ;
                else if (current[i] == 2) ;
                else if (current[i] == 3) ;
                //else if (current[i] == 15) translate[i] = 15;
                else continue;

                int x = i % 4;
                int y = i / 4;

                int ox = current[i] % 4;
                int oy = current[i] / 4;

                int rx = Math.Abs(ox - x);
                int ry = Math.Abs(oy - y);
                sum -= (rx + ry);
            }

            return sum;
        }

    }

    public class InvalidCombinationException : Exception
    {
        public int dimension = 0;
        public InvalidCombinationException(int dimension)
            : base("The combination is invalid.")
        {
            this.dimension = dimension;
        }
    }
}
