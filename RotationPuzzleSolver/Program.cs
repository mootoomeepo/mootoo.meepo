using System;
using System.Collections.Generic;
using System.Text;
using RotationPuzzle;

namespace RotationPuzzleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int d = 3;
            int[] p = new int[d * d];

            for (int i = 0; i < p.Length; i++) p[i] = i;

            RotationPuzzle.RotationPuzzleSolver.Move m = new RotationPuzzle.RotationPuzzleSolver.Move(p, "XXX", null, d, d);

            int d2 = (d-1)*(d-1);
            for (int i=0;i<d2;i++)
                m = m.GenMove(i, RotationPuzzle.RotationPuzzleSolver.Move.Direction.CLOCKWISE);

            for (int i = 0; i < d2; i++)
                m = m.GenMove(i, RotationPuzzle.RotationPuzzleSolver.Move.Direction.COUNTER_CLOCKWISE);


            for (int i = 0; i < m.current.Length; i++)
                Console.Write(m.current[i] + ",");
            Console.WriteLine();

            int countStep = 0;
            string[] steps = RotationPuzzleSolver.Process(d, d, m.current, ref countStep);

            for (int i=0;i<steps.Length;i++)
            {
                Console.WriteLine(steps[i]);

                int pos = int.Parse(steps[i].Substring(0, 2));
                string direction = steps[i].Substring(2, 1);

                m = m.GenMove(pos, ((direction == "T") ? RotationPuzzle.RotationPuzzleSolver.Move.Direction.COUNTER_CLOCKWISE : RotationPuzzle.RotationPuzzleSolver.Move.Direction.CLOCKWISE));

                for (int k = 0; k < m.current.Length; k++)
                    Console.Write(m.current[k] + ",");
                Console.WriteLine();
            }

            for (int i = 0; i < m.current.Length; i++)
                Console.Write(m.current[i] + ",");
            Console.WriteLine();

            Console.ReadLine();
        }
    }
}
