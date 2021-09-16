using System;
using System.Collections.Generic;

namespace LearningCSharp
{
    public class RecamanSequence
    {
        public static int[] Recaman(int upTo)
        {
            int[] result = new int[upTo];
            HashSet<int> already = new HashSet<int>();
            already.Add(0);

            for (int i = 1; i < upTo - 1; i++)
            {
                int c = result[i - 1] - i;

                if (c < 0 || already.Contains(c)) c = result[i - 1] + i;

                result[i] = c;
                already.Add(c);
            }

            return result;
        }

        public static void Test(int upTo)
        {
            Console.Write($"Recaman sequence up to {upTo}: ");

            int[] result = Recaman(upTo);

            for (int i = 0; i < result.Length; i++)
            {
                Console.Write(result[i]);

                if (i < result.Length - 1) Console.Write(", ");
            }

            Console.WriteLine();
        } 
    }
}