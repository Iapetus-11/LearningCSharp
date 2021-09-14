using System;
using System.Numerics;

namespace MyFirstSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            int fibonacciUpTo = 1_000_000;
            DateTime startTime = DateTime.Now;

            FibonacciMatrix fibonacciMatrixRunner = new FibonacciMatrix();
            BigInteger result = fibonacciMatrixRunner.Fibonacci(fibonacciUpTo);
            
            Console.WriteLine(result);
            Console.WriteLine($"FibonacciMatrix.fibonacci({fibonacciUpTo}) took {(DateTime.Now - startTime).TotalSeconds} seconds");
        }
    }
}
