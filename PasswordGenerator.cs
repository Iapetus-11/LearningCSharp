using System;
using System.Text;

namespace LearningCSharp
{
    public class PasswordGenerator
    {
        private static char[] chars =
            "abcdefhijklmnopqrstuvwxyzABCDEFHIJKLMNOPQRSTUVWXYZ123456789&%$#@!+-=".ToCharArray();

        private static string GeneratePassword(int length)
        {
            var password = new StringBuilder();
            var random = new Random();
            
            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(0, chars.Length)]);
            }

            return password.ToString();
        }

        public static void Test(int n, int length)
        {
            var startTime = DateTime.Now;
            
            for (int i = 0; i < n; i++)
            {
                GeneratePassword(length);
            }
            
            var timeTaken = DateTime.Now - startTime;
            
            Console.WriteLine($"PasswordGenerator.GeneratePassword({length}) x ${n} took {timeTaken.TotalSeconds} seconds");
            Console.WriteLine($"Sample: \"{GeneratePassword(length)}\"");
        }
    }
}