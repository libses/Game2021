//302578XJ
using System;
using System.Collections.Generic;
using System.Linq;

namespace Program {
    class Program
    { 
        static void Print(int[,] array, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(array[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public static void Main()
        {
            var input = Console.ReadLine().Split();
            var n = int.Parse(input[0]);
            var startVertex = int.Parse(input[1]);

            var matrix = new int[n,n];

            for(int i = 0; i < n; i++)
            {
                var strArr = Console.ReadLine().Split();
                for(int j = 0; j < n; j++)
                {
                    matrix[i, j] = int.Parse(strArr[j]);
                }
            }

            Print(matrix, n);
        }
    }
}