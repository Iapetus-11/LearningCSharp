using System;
using System.Numerics;

namespace MyFirstSolution
{
    public class FibonacciMatrix
    {
        // private BigInteger[] _mat = { 0, 0, 0, 0 };

        private BigInteger[] _mult(BigInteger[] x, BigInteger[] y)
        {
            return new BigInteger[]
            {
                x[0] * x[1] + x[1] * y[2],
                x[0] * y[1] + x[1] * y[3],
                x[2] * y[0] + x[3] * y[2],
                x[2] * y[1] + x[3] * y[3]
            };
        }

        private BigInteger[] _pow(BigInteger[] x, int n)
        {
            if (n == 0) return new BigInteger[] { 1, 0, 0, 1 };
            if (n == 1) return x;
            if (n % 2 == 0) return _pow(_mult(x, x), n / 2);

            return _mult(x, _pow(x, n - 1));
        }

        private BigInteger Fibonacci(int n)
        {
            BigInteger[] initial = {0, 1, 1, 1};
            return _pow(initial, n)[1];
        }

        public static void Test()
        {
            int fibonacciUpTo = 1_000_000;
            DateTime startTime = DateTime.Now;

            FibonacciMatrix fibonacciMatrixRunner = new FibonacciMatrix();
            BigInteger result = fibonacciMatrixRunner.Fibonacci(fibonacciUpTo);
            TimeSpan timeTaken = DateTime.Now - startTime;

            Console.WriteLine(result);
            Console.WriteLine($"FibonacciMatrix.fibonacci({fibonacciUpTo}) took {timeTaken.TotalSeconds} seconds");
        }
    }
}