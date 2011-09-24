using System;
using System.Collections.Generic;
using System.Text;

namespace AutoSlidingTile
{
    class Test44
    {
        public static void Main()
        {
            int N = 50;
            int sumCount = 0;
            int maxCount = 0;

            int maxStep = 0;
            int sumStep = 0;

            int skip = 6;

            int number = 0;

            int[] pieces = new int[] { 0, 1, 2, 3, 4, 5, 6 ,7 ,8,9,10,11,12,13,14,15 };

            for (int i = 0; i < N; i++)
            {
                int[] steps = null;
                int count = 0;
                while (true)
                {
                    count = 0;
                    try
                    {
                        RandomlySwap(pieces);

                        //Console.WriteLine();
                        //Console.WriteLine();
                        //Console.WriteLine("--- "+(number++)+" ---");
                        //Print(pieces);

                        bool invalid = false;
                        if (skip > 0)
                        {
                            skip--;
                            invalid = true;
                        }

                        for (int k = 0; k < pieces.Length; k++)
                        {
                            if (pieces[k] == 15)
                            {
                                if (0 <= k && k <= 3) invalid = true;
                                else if (k == 4) invalid = true;
                                else if (k == 8) invalid = true;
                                else if (k == 12) invalid = true;
                            }
                        }

                        if (invalid == true) continue;


                        steps = JigsawSolver.Process(4, pieces, ref count);

                        if (steps == null)
                        {
                            Console.WriteLine("Solveable, but program cannot solve.");
                            continue;
                        }

                        break;
                    }
                    catch (InvalidCombinationException e)
                    {
                        if (e.dimension == 3)
                        {
                            Console.WriteLine("Unsolveable "+e.dimension);

                        
                            Console.ReadKey();
                        }
                    }
                }

                sumStep += steps.Length;
                if (steps.Length > maxStep) maxStep = steps.Length;

                sumCount += count;
                if (count > maxCount) maxCount = count;
                Console.WriteLine(i + ". searchSteps = " + count + ", steps =" + steps.Length);

                //for (int y = 0; y < steps.Length; y++)
                //{
                //    Console.Write(steps[y] + ",");
                //}
                //Console.ReadLine();
            }

            Console.WriteLine("avg = " + (sumCount / N));
            Console.WriteLine("max = " + maxCount);

            Console.WriteLine("step avg = " + (sumStep / N));
            Console.WriteLine("step max = " + maxStep);

            Console.ReadKey();
        }

        private static Random rand = new Random(0);
        private static void RandomlySwap(int[] pieces)
        {
            int srcPos = rand.Next(pieces.Length);
            int destPos = rand.Next(pieces.Length);

            while (srcPos == destPos)
                destPos = rand.Next(pieces.Length);

            int temp = pieces[srcPos];
            pieces[srcPos] = pieces[destPos];
            pieces[destPos] = temp;
        }

        private static void Print(int[] pieces)
        {
            for (int i = 0; i < pieces.Length; i++)
            {
                if ((i % 4) == 0) Console.WriteLine();

               Console.Write(AddSpace(pieces[i]) + ",");
            }

            Console.WriteLine();
        }

        private static string AddSpace(int p)
        {
            if (p < 10) return " " + p;
            else return "" + p;
        }


    }
}
