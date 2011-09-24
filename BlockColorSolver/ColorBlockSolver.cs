using System;
using System.Collections.Generic;
using System.Text;
using AutoSlidingTile;
using System.Collections;

namespace ColorBlock
{
    public class ColorBlockSolver
    {

        public static string[] Solve(int[,] initial, ref int countStep, ref Move refAnswer, int maxStep, long durationPerClickMs, long timeLimitMs)
        {
            long startTime = DateTime.Now.Ticks;

            int[] count = new int[maxStep + 1];
            int[] countScore = new int[10];
            Move bestAnswer = null;
            Move[] answer = new Move[6];

            BinaryHeap[] nextQueue = new BinaryHeap[6];
            for (int color = 1; color < 6; color++)
                nextQueue[color] = new BinaryHeap();

            Hashtable used = new Hashtable();

            for (int color=1;color<6;color++)
            {
                int[,] initialColor = new int[9, 9];
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        initialColor[i, j] = (initial[i, j] == color) ? color : 0;

                Move firstMove = new Move(initialColor, null, null);
                answer[color] = firstMove;
                nextQueue[color].Insert(firstMove.level * -100 + firstMove.score, firstMove);
                used[firstMove.signature] = true;
            }

            

            while (true)
            {
                bool isBreak = true;
                for (int color = 1; color < 6; color++)
                    if (nextQueue[color].Count > 0) isBreak = false;

                if (isBreak == true) break;

                countStep++;

                bool end = false;
                for (int color = 1; color < 6; color++)
                {
                    Move m = (Move)(nextQueue[color].Remove());
                    if (m.score == 9)
                    {
                        answer[color] = m;
                        end = true;
                        break;
                    }

                    Move[] newMoves = new Move[] {
                        m.next(0,'U'),m.next(1,'U'), m.next(2,'U'),
                        m.next(0,'D'),m.next(1,'D'), m.next(2,'D'),
                        m.next(0,'R'),m.next(1,'R'), m.next(2,'R'),
                        m.next(0,'L'),m.next(1,'L'), m.next(2,'L')
                    };

                    
                    for (int i = 0; i < newMoves.Length; i++)
                    {
                        if (newMoves[i].score == 9)
                        {
                            answer[color] = newMoves[i];
                            end = true;
                            break;
                        }

                        if ((newMoves[i].score > answer[color].score && newMoves[i].level == maxStep)
                            || newMoves[i].score == 9)
                            answer[color] = newMoves[i];

                        if (used[newMoves[i].signature] != null
                            && answer[color] != newMoves[i]) continue;

                        count[newMoves[i].level]++;
                        countScore[newMoves[i].score]++;

                        used[newMoves[i].signature] = true;

                        if (newMoves[i].level >= maxStep) continue;
                        nextQueue[color].Insert(newMoves[i].level*-100 + newMoves[i].score, newMoves[i]);

                    }
                    
                    if (end == true) break;
                }


                Console.Clear();

                for (int color = 1; color < 6; color++)
                {
                    Console.WriteLine("Optimal steps = " + answer[color].level + " score=" + answer[color].score);
                    Console.WriteLine();
                    answer[color].Print();
                    Console.WriteLine();

                    Console.WriteLine("Time used = " + ((answer[color].ticks - startTime) / TimeSpan.TicksPerMillisecond) + "ms");
                }

                

                for (int color = 1; color < 6; color++)
                {
                    if (bestAnswer == null
                        || (answer[color].score == 9)
                        || (bestAnswer.score < answer[color].score && answer[color].level == maxStep))
                    {
                        bestAnswer = answer[color];
                    }
                }

                long timeUsed = ((DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond);
                //Console.WriteLine("Since beginning = " + timeUsed + "ms");
                if (((bestAnswer.level * durationPerClickMs) + timeUsed) > (timeLimitMs - 1000)) break;

                if (end == true) break;
            }

            for (int color = 1; color < 6; color++)
            {
                if (bestAnswer == null || bestAnswer.score < answer[color].score
                    || (bestAnswer.score == answer[color].score && bestAnswer.level > answer[color].level))
                {
                    bestAnswer = answer[color];
                }
            }

            refAnswer = bestAnswer;
            string[] steps = new string[bestAnswer.level];

            for (int i = steps.Length-1; i >= 0; i--)
            {
                steps[i] = bestAnswer.move;
                bestAnswer = bestAnswer.parent;
            }

            return steps;
        }


    }

    public class Move
    {
        public int[,] p;
        public string move;
        public Move parent;
        public int level;
        public int score;

        public long ticks;

        public string signature;

        public Move(int[,] p,string move, Move parent)
        {
            ticks = DateTime.Now.Ticks;

            if (parent == null) level = 0;
            else level = parent.level + 1;

            this.parent = parent;
            this.p = p;
            this.move = move;
            score = GetScore(p);
            signature = GenSignature();
        }

        private string GenSignature()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sb.Append(p[i,j]);
                }
            }

            return sb.ToString();
        }



        private static int GetScore(int[,] p)
        {
            int[] colors = new int[6];


            for (int i = 3; i < 6; i++)
            {
                for (int j = 3; j < 6; j++)
                {
                    if (i == 4 || j == 4)
                        colors[p[i,j]]++;
                }
            }

            int max = 0;
            for (int i = 1; i < colors.Length; i++)
            {
                if (colors[i] == 5)
                {
                    if (p[3,3] == i) colors[i]++;
                    if (p[3,5] == i) colors[i]++;
                    if (p[5,3] == i) colors[i]++;
                    if (p[5,5] == i) colors[i]++;
                }

                if (colors[i] > max) max = colors[i];
            }

            if (max >= 7) max = 9;
            return max;
        }
            
        public Move next(int index,char direction)
        {
            int[,] newP = new int[9,9];
            for (int i=0;i<9;i++)
                for (int j=0;j<9;j++)
                    newP[i,j] = p[i,j];

            if (direction == 'U')
            {
                int col = index + 3;

                newP[8,col] = p[0,col];
                for (int i=0;i<8;i++)
                {
                    newP[i,col] = p[i+1,col];
                }
            }
            else if (direction == 'D')
            {
                int col = index + 3;

                newP[0,col] = p[8,col];
                for (int i=1;i<9;i++)
                {
                    newP[i,col] = p[i-1,col];
                }
            }
            else if (direction == 'L')
            {
                int row = index + 3;

                newP[row,8] = p[row,0];
                for (int i=0;i<8;i++)
                {
                    newP[row,i] = p[row,i+1];
                }
            }
            else if (direction == 'R')
            {
                int row = index + 3;

                newP[row,0] = p[row,8];
                for (int i=1;i<9;i++)
                {
                    newP[row,i] = p[row,i-1];
                }
            }

            return new Move(newP,index+""+direction,this);
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
