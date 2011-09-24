using System;
using System.Collections.Generic;
using System.Text;

namespace RotationPuzzle
{
    public class ProgramTest
    {
        static void Main(string[] args)
        {
            int d = 4;
            int N = 1000;

            int[] p = new int[d*d];

            for (int i = 0; i < p.Length; i++) p[i] = i;

            int sumSearch = 0;
            int sumStep = 0;
            long sumTime = 0;
            for (int i = 0; i < N; i++)
            {
                RandomlySwap(p);

                int countStep = 0;
                long start = DateTime.Now.Ticks;
                string[] steps = RotationPuzzleSolver.Process(d, d, p, ref countStep);

                sumStep += steps.Length;
                sumSearch += countStep;

                long timeUsed = ((DateTime.Now.Ticks - start) / TimeSpan.TicksPerMillisecond);
                long totalTimeUsed = (timeUsed + steps.Length*300);

                sumTime += totalTimeUsed;

                Console.WriteLine(i + ". steps=" + steps.Length + ", search=" + countStep + ", t=" + timeUsed + ", p="+totalTimeUsed);
                //for (int i = 0; i < steps.Length; i++)
                //{
                //    Console.WriteLine(steps[i]);
                //}

                //Console.ReadLine();
            }

            Console.WriteLine("avg. steps = "+(sumStep/N)+", avg. search = "+(sumSearch/N)  +", timeUsed="+(sumTime/N));

            Console.ReadLine();
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
    }
}
