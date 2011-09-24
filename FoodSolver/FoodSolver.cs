using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace FoodSolverNameSpace
{
    public class FoodSolver
    {
        public static string[] Solve(int[] first, int[] second,int[] third, int[] spoon_limits, ref int countStep)
        {
            int N = first.Length;
            if (second.Length > N) N = second.Length;
            if (third.Length > N) N = third.Length;

            List<string> steps = new List<string>();
            for (int i = 0; i < N; i++)
            {
                if (i < first.Length) steps.AddRange(PerformSingle(first[i], spoon_limits,0,i));
                if (i < second.Length) steps.AddRange(PerformSingle(second[i], spoon_limits, 1, i));
                if (i < third.Length) steps.AddRange(PerformSingle(third[i], spoon_limits, 2, i));

            }

            for (int i = steps.Count - 1; i >= 0; i--)
            {
                char[] c = steps[i].ToCharArray();

                if (c[1] == 'S') break;

                steps.RemoveAt(i);
            }

            return steps.ToArray();
        }

        private static string[] PerformSingle(int food_limits, int[] spoon_limits, int dish_number,int food_number)
        {
            int countSteps = 0;
            string[] steps = Solve(food_limits, spoon_limits, ref countSteps);

            for (int j = 0; j < steps.Length; j++)
            {
                char[] c = steps[j].ToCharArray();

                if (c[1] == 'F') steps[j] = c[0] + "" + c[1] + "" + food_number;
                else if (c[1] == 'S') steps[j] = c[0] + "" + c[1] + "" + dish_number;
            }

            return steps;
        }

        public static string[] Solve(int food_limit, int[] spoon_limits, ref int countStep)
        {
            Queue<Move> nextQueue = new Queue<Move>();
            Hashtable used = new Hashtable();

            Move finish = null;

            int[] spoons = new int[spoon_limits.Length];
            for (int i = 0; i < spoons.Length; i++) spoons[i] = 0;

            Move firstMove = new Move(0, spoons, food_limit, spoon_limits, "", null);
            nextQueue.Enqueue(firstMove);

            used[firstMove.signature] = true;

            while (true)
            {
                if (nextQueue.Count == 0) break;

                countStep++;
                Move m = (Move)(nextQueue.Dequeue());
                if (m.IsFinished)
                {
                    finish = m;
                    break;
                }

                Move[] newMoves = m.GenNextMoves();

                for (int i = 0; i < newMoves.Length; i++)
                {
                    if (newMoves[i] == null) continue;

                    if (newMoves[i].IsFinished)
                    {
                        finish = newMoves[i];
                        break;
                    }

                    if (used[newMoves[i].signature] != null) continue;

                    used[newMoves[i].signature] = true;

                    nextQueue.Enqueue(newMoves[i]);
                }

                if (finish != null) break;
            }

            if (finish == null) return null;

            List<string> steps = new List<string>();

            while (finish != null)
            {
                //Console.WriteLine(finish.ToString());
                steps.Insert(0, finish.move);
                finish = finish.parent;

                
            }

            steps.RemoveAt(0);

            return steps.ToArray();
        }
    }

    public class Move
    {

        public int food;
        public int[] spoons;

        public int food_limit;
        public int[] spoon_limits;

        public Move parent;
        public int level;
        public int score;

        public bool IsFinished = false;
        public string signature;

        public string move;

        public Move(int food,int[] spoons,int food_limit,int[] spoon_limits,string move,Move parent)
        {
            if (parent == null) level = 0;
            else level = parent.level + 1;

            this.food = food;
            this.spoons = spoons;
            this.food_limit = food_limit;
            this.spoon_limits = spoon_limits;
            this.move = move;
            this.parent = parent;

            IsFinished = (food == food_limit)?true:false;

            for (int i = 0; i < spoons.Length;i++ )
            {
                if (spoons[i] > 0) IsFinished = false;
            }

            signature = GetSignature();

            score = -level;

        }

        private string GetSignature()
        {
            StringBuilder s = new StringBuilder();

            s.Append(food);

            for (int i = 0; i < spoons.Length; i++) s.Append(spoons[i] + ",");

            return s.ToString();
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append("(" +food+")");

            s.Append(" ");
            for (int i = 0; i < spoons.Length; i++) s.Append("[" + spoons[i] + "] ");

            s.Append(" " + move);

            return s.ToString();
        }


        internal Move[] GenNextMoves()
        {
            List<Move> moves = new List<Move>();

            for (int i = 0; i < spoons.Length; i++)
            {

                // from food
                {
                    int[] new_spoons = Copy(spoons);

                    if (new_spoons[i] > 0) new_spoons[i] = 0;
                    else new_spoons[i] = spoon_limits[i];

                    moves.Add(new Move(food, new_spoons, food_limit, spoon_limits, i+"F-", this));
                }

                if (spoons[i] == 0) continue;


                // to other spoons
                for (int j = 0; j < spoons.Length; j++)
                {
                    if (i == j) continue;
                    if (spoons[j] == spoon_limits[j]) continue;

                    int[] new_spoons = Copy(spoons);

                    if ((spoon_limits[j] - spoons[j]) >= spoons[i])
                    {
                        new_spoons[j] += spoons[i];
                        new_spoons[i] = 0;
                    }
                    else
                    {
                        new_spoons[j] = spoon_limits[j];
                        new_spoons[i] -= spoon_limits[j] - spoons[j];
                    }

                    moves.Add(new Move(food, new_spoons, food_limit, spoon_limits, i + "P"+j, this));
                }

                // to food set
                {
                    if ((food_limit - food) < spoons[i])
                        continue;


                    int[] new_spoons = Copy(spoons);

                    int new_food = food + spoons[i];
                    new_spoons[i] = 0;

                    moves.Add(new Move(new_food, new_spoons, food_limit, spoon_limits, i + "S-", this));
                }
            }

            return moves.ToArray();
        }

        private int[] Copy(int[] x)
        {
            int[] copying = new int[x.Length];
            for (int i = 0; i < copying.Length; i++) copying[i] = x[i];

            return copying;
        }
    }
}
