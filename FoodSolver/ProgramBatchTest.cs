using System;
using System.Collections.Generic;
using System.Text;

namespace FoodSolverNameSpace
{

    class ProgramBatchTest
    {
        static void Main(string[] args)
        {
            int[] spoon_limits = new int[] { 0,0,0};

            int count = 0;
            int sumStep = 0;
            int maxStep = 0;

            int sumSearchStep = 0;
            int maxSearchStep = 0;

            for (int i = 9; i >=0; i--)
            {
                Console.WriteLine("num = " + i);

                for (int a=2;a<10;a++)
                    for (int b=a+1;b<10;b++)
                        for (int c = 2; c < 10; c++)
                        {
                            spoon_limits[0] = c;
                            spoon_limits[1] = b;
                            spoon_limits[2] = a;

                            int countStep = 0;
                            string[] steps = FoodSolver.Solve(i, spoon_limits, ref countStep);

                            if (steps == null)
                            {
                                //Console.WriteLine("Cannot solve " + i + " " + (c*100+b*10+a));
                                continue;
                            }

                            sumSearchStep += countStep;
                            if (countStep > maxSearchStep) maxSearchStep = countStep;

                            sumStep += steps.Length;
                            if (steps.Length > maxStep) maxStep = steps.Length;

                            count++;


                            //for (int i = 0; i < steps.Length; i++)
                            //{
                            //    Console.WriteLine(steps[i]);
                            //}

                        }

                Console.WriteLine("max search steps = " + maxSearchStep);
                Console.WriteLine("avg. search steps = " + (sumSearchStep / count));
                Console.WriteLine("max steps = " + maxStep);
                Console.WriteLine("avg. steps = " + (sumStep / count));
                Console.WriteLine("--");
            }

            Console.WriteLine("max search steps = " + maxSearchStep);
            Console.WriteLine("avg. search steps = " + (sumSearchStep / count));
            Console.WriteLine("max steps = " + maxStep);
            Console.WriteLine("avg. steps = " + (sumStep / count));

            Console.ReadLine();
        }
    }
}
