using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator
{
    class Program
    {
        static void DataGenerator1()
        {
            Random rnd = new Random();

            for (int i = 0; i < 4; i++)
            {
                int x = rnd.Next(100);
                int y = rnd.Next(100);

                for (int j = 0; j < 10; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        Console.WriteLine($"{x + j},{y + k}");
                    }
                }
            }
        }

        static void DataGenerator2()
        {
            Random rnd = new Random();

            for (int i = 0; i < 4; i++)
            {
                int x = rnd.Next(1000);
                int y = rnd.Next(1000);
                int r = rnd.Next(20) + 50;

                for (int j = 0; j < 40; j++)
                {
                    double v1 = rnd.Next(r);
                    double v2 = Math.Sqrt(r * r - v1 * v1) - rnd.Next(r);

                    Console.WriteLine($"{x + v1},{y + Math.Truncate(v2)}");
                    Console.WriteLine($"{x - v1},{y + Math.Truncate(v2)}");
                    Console.WriteLine($"{x + v1},{y - Math.Truncate(v2)}");
                    Console.WriteLine($"{x - v1},{y - Math.Truncate(v2)}");
                }
            }
        }

        static void DataGenerator3()
        {
            Random rnd = new Random();

            for (int i = 0; i < 5; i++)
            {
                int x = rnd.Next(1000);
                int y = rnd.Next(1000);
                int r = rnd.Next(20) + 50;

                for (int j = 0; j < 100; j++)
                {
                    double v1 = rnd.Next(r);
                    double v2 = Math.Sqrt(r * r - v1 * v1) - rnd.Next(r);

                    Console.WriteLine($"{x + v1},{y + Math.Truncate(v2)}");
                    Console.WriteLine($"{x - v1},{y + Math.Truncate(v2)}");
                    Console.WriteLine($"{x + v1},{y - Math.Truncate(v2)}");
                    Console.WriteLine($"{x - v1},{y - Math.Truncate(v2)}");
                }
            }
        }

        static void DataGenerator4()
        {
            Random rnd = new Random();

            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine($"{rnd.Next(100) + 10},{rnd.Next(100) + 10}");
            }
        }

        static void Main(string[] args)
        {
            // This program is not the part of the main project...
            //DataGenerator1();
            //DataGenerator2();
            //DataGenerator3();
            DataGenerator4();
        }
    }
}
