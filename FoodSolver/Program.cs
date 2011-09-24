using System;
using System.Collections.Generic;
using System.Text;

namespace FoodSolverNameSpace
{
    class Program
    {
        public static void Main(string[] args)
        {
            int[] first = new int[] {7,5,0 };
            int[] second = new int[] { 1, 1, 0 };
            int[] third = new int[] { 1, 1, 0 };
            int[] spoons = new int[] { 7, 6, 4 };

            int countStep = 0;
            string[] steps = FoodSolver.Solve(first, second, third, spoons, ref countStep);

            for (int i = 0; i < steps.Length; i++)
            {
                Console.WriteLine(steps[i]);
            }

            Console.ReadLine();
        }
    }
}
