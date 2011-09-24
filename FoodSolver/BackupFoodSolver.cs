using System;
using System.Collections.Generic;
using System.Text;
using AutoSlidingTile;
using System.Collections;

namespace FoodSolverNameSpace
{
    public class ThreeFoodSolver
    {
        public static string[] Solve(int[] food_limits, int[] spoon_limits,ref int countStep)
        {
            BinaryHeap nextQueue = new BinaryHeap();
            Hashtable used = new Hashtable();

            Move finish = null;

            int[] foods = new int[food_limits.Length];
            for (int i = 0; i < foods.Length; i++) foods[i] = 0;

            int[] spoons = new int[spoon_limits.Length];
            for (int i = 0; i < spoons.Length; i++) spoons[i] = 0;

            Move firstMove = new Move(foods,spoons,food_limits,spoon_limits,"",null);
            nextQueue.Insert(firstMove.score, firstMove);

            used[firstMove.signature] = true;

            while (true)
            {
                if (nextQueue.Count == 0) break;

                countStep++;
                Move m = (Move)(nextQueue.Remove());
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

                    nextQueue.Insert(newMoves[i].score, newMoves[i]);
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

        public int[] foods;
        public int[] spoons;

        public int[] food_limits;
        public int[] spoon_limits;

        public Move parent;
        public int level;
        public int score;

        public bool IsFinished = false;
        public string signature;

        public string move;

        public Move(int[] foods,int[] spoons,int[] food_limits,int[] spoon_limits,string move,Move parent)
        {
            if (parent == null) level = 0;
            else level = parent.level + 1;

            this.foods = foods;
            this.spoons = spoons;
            this.food_limits = food_limits;
            this.spoon_limits = spoon_limits;
            this.move = move;
            this.parent = parent;

            IsFinished = true;
            for (int i = 0; i < foods.Length; i++)
            {
                if (foods[i] != food_limits[i])
                {
                    IsFinished = false;
                }
            }

            signature = GetSignature();

            score = -level;

            //for (int i = 0; i < foods.Length; i++)
            //{
            //    score = foods[i] - food_limits[i];
            //}
        }

        private string GetSignature()
        {
            StringBuilder s = new StringBuilder();

            for (int i = 0; i < foods.Length; i++) s.Append(foods[i] + ",");

            for (int i = 0; i < spoons.Length; i++) s.Append(spoons[i] + ",");

            return s.ToString();
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append("(");
            for (int i = 0; i < foods.Length; i++) s.Append(foods[i] + ",");

            s.Append(")");

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
                    int[] new_foods = Copy(foods);
                    int[] new_spoons = Copy(spoons);

                    if (new_spoons[i] > 0) new_spoons[i] = 0;
                    else new_spoons[i] = spoon_limits[i];

                    moves.Add(new Move(new_foods, new_spoons, food_limits, spoon_limits, i+"F-", this));
                }

                if (spoons[i] == 0) continue;


                // to other spoons
                for (int j = 0; j < spoons.Length; j++)
                {
                    if (i == j) continue;
                    if (spoons[j] == spoon_limits[j]) continue;

                    int[] new_foods = Copy(foods);
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

                    moves.Add(new Move(new_foods, new_spoons, food_limits, spoon_limits, i + "P"+j, this));
                }

                // to food set
                for (int j = 0; j < foods.Length; j++)
                {
                    if ((food_limits[j] - foods[j]) < spoons[i])
                        continue;


                    int[] new_foods = Copy(foods);
                    int[] new_spoons = Copy(spoons);

                    new_foods[j] += spoons[i];
                    new_spoons[i] = 0;

                    moves.Add(new Move(new_foods, new_spoons, food_limits, spoon_limits, i + "S" + j, this));
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
