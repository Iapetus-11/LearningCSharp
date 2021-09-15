using System;
using System.Numerics;

namespace LearningCSharp
{
    public class FibonacciMatrix
    {
        private static BigInteger[] _mult(BigInteger[] x, BigInteger[] y)
        {
            return new BigInteger[]
            {
                x[0] * x[1] + x[1] * y[2],
                x[0] * y[1] + x[1] * y[3],
                x[2] * y[0] + x[3] * y[2],
                x[2] * y[1] + x[3] * y[3]
            };
        }

        private static BigInteger[] _pow(BigInteger[] x, int n)
        {
            if (n == 0) return new BigInteger[] { 1, 0, 0, 1 };
            if (n == 1) return x;
            if (n % 2 == 0) return _pow(_mult(x, x), n / 2);

            return _mult(x, _pow(x, n - 1));
        }

        private BigInteger _fibonacci(int n)
        {
            BigInteger[] initial = {0, 1, 1, 1};
            return _pow(initial, n)[1];
        }

        public void Test(int n)
        {
            DateTime startTime = DateTime.Now;

            BigInteger result = _fibonacci(n);
            TimeSpan timeTaken = DateTime.Now - startTime;

            Console.WriteLine(result);
            Console.WriteLine($"FibonacciMatrix.fibonacci({n}) took {timeTaken.TotalSeconds} seconds");
        }
    }
}