using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningCSharp
{
    public class Anagram
    {
        private static string[,] _testWords =
        {
            {"one", "noe"},
            {"min", "nim"},
            {"anagram", "margana"},
            {"a gentleman", "elegant man"},
            {"aa", "aaa"},
            {"a", "b"},
            {"bruh", "broooo"}
        };
        
        private static bool _isAnagram(string x, string y)
        {
            var xAsSet = x.ToHashSet();
            xAsSet.SymmetricExceptWith(y);
            
            return x.Length == y.Length && xAsSet.Count == 0;
        }

        private static void _test(string x, string y)
        {
            Console.WriteLine($"_isAnagram(\"{x}\", \"{y}\") == {_isAnagram(x, y)}");
        }

        public static void Test()
        {
            for (int i = 0; i < _testWords.GetLength(0); i++)
            {
                _test(_testWords[i, 0], _testWords[i, 1]);
            }
        }
    }
}