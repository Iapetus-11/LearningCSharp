using System;
using System.Text;

namespace MyFirstSolution
{
    public class PasswordGenerator
    {
        private static char[] chars =
            "abcdefhijklmnopqrstuvwxyzABCDEFHIJKLMNOPQRSTUVWXYZ123456789&%$#@!+-=".ToCharArray();

        public static string GeneratePassword(int length)
        {
            StringBuilder password = new StringBuilder();
            Random random = new Random();
            
            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(0, chars.Length)]);
            }

            return password.ToString();
        }

        public static void Test()
        {
            int iterations = 100_000;
            int length = 10;
            DateTime startTime = DateTime.Now;
            
            for (int i = 0; i < iterations; i++)
            {
                GeneratePassword(length);
            }
            
            TimeSpan timeTaken = DateTime.Now - startTime;
            
            Console.WriteLine($"PasswordGenerator.GeneratePassword({iterations}) x ${iterations} took {timeTaken.TotalSeconds} seconds");
            Console.WriteLine($"Sample: \"{GeneratePassword(length)}\"");
        }
    }
}