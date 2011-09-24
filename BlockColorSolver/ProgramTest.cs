using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace ColorBlock
{
    class ProgramTest
    {
        public static void Main(string[] args)
        {
            

            int[,] p = new int[9, 9] {
                {0,0,0,1,1,5,0,0,0},
                {0,0,0,1,4,3,0,0,0},
                {0,0,0,4,2,5,0,0,0},
                {5,3,3,5,1,1,4,3,5},
                {5,5,1,2,2,2,4,2,2},
                {2,5,4,2,3,4,1,1,3},
                {0,0,0,3,3,5,0,0,0},
                {0,0,0,2,4,4,0,0,0},
                {0,0,0,4,3,1,0,0,0}
            };

            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            currentDir = currentDir.Replace("file:\\", "");
            TextWriter tw = new StreamWriter(currentDir+"/result.txt");

            tw.WriteLine("steps,avg. time,avg. step,false");
            Console.WriteLine("steps,avg. time,avg. step,false");

            for (int max = 16; max >= 16; max--)
            {
                for (long timeLimit = 100000; timeLimit <= 100000; timeLimit += 5000)
                {
                    int[] success = new int[10];
                    int sum_steps = 0;
                    long sum_used_time = 0;
                    int num_false = 0;
                    int N = 100;

                    for (int i = 0; i < N; i++)
                    {
                        for (int k = 0; k < 10; k++)
                            RandomlySwap(p);

                        int countStep = 0;
                        long usedTime = System.DateTime.Now.Ticks;
                        string[] steps = AvgPathColorBlockSolver.Solve(p, ref countStep, max, 200, timeLimit);

                        if (steps == null)
                        {
                            num_false++;
                            Console.WriteLine(i + ". -> null");

                            for (int a = 0; a < 9; a++)
                            {
                                for (int b = 0; b < 9; b++)
                                {
                                    Console.Write(p[a, b] + ",");
                                }
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            sum_steps += steps.Length;
                            Console.WriteLine(i + ". -> " + steps.Length);
                        }

                        sum_used_time += System.DateTime.Now.Ticks - usedTime;

                    }

                    tw.Write("" + max + "," + (sum_used_time / N)/TimeSpan.TicksPerMillisecond + "," + (sum_steps / N) + ","+num_false+"/"+N);
                    Console.Write("" + max + "," + (sum_used_time / N) / TimeSpan.TicksPerMillisecond + "," + (sum_steps / N) + ","+num_false+"/"+N);
                    //for (int i = success.Length-1; i >=0; i--)
                    //{
                    //    if (success[i] > 0)
                    //    {
                    //        tw.Write(success[i] + ",");
                    //        Console.Write(success[i] + ",");
                    //    }
                    //    else
                    //    {
                    //        tw.Write("-,");
                    //        Console.Write("-,");
                    //    }
                    //}

                    Console.WriteLine();
                    tw.WriteLine();
                    tw.Flush();

                    if (success[9] > 90)
                    {
                        tw.WriteLine("--- Stop testing for " + max + " steps because the result is already good. ---");
                        Console.WriteLine("--- Stop testing for " + max + " steps because the result is already good. ---");
                        break;
                    }

                    
                }

                Console.WriteLine();
                tw.WriteLine();
            }
            tw.Close();
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static Random rand = new Random(0);
        public static void RandomlySwap(int[,] p)
        {
            int srcPos = rand.Next(45);
            int destPos = rand.Next(45);

            while (srcPos == destPos)
            {
                destPos = rand.Next(9);
            }

            int[] srcCoord = GetCoord(srcPos);
            int[] destCoord = GetCoord(destPos);

            int temp = p[srcCoord[0], srcCoord[1]];
            p[srcCoord[0], srcCoord[1]] = p[destCoord[0], destCoord[1]];
            p[destCoord[0], destCoord[1]] = temp;
        }

        private static int[] GetCoord(int pos)
        {
            if (0 <= pos && pos <= 8)
            {
                return new int[2] { (pos/3),(pos%3)+3 };
            }
            else if (36 <= pos && pos < 45)
            {
                return new int[2] { ((pos - 36) / 3) + 6, ((pos - 36) % 3) + 3 };
            }
            else
            {
                return new int[2] { 3 + (pos - 9) / 9, ((pos - 9) %9)};
            }
        }
    }
}
