using System;
using System.Linq;
using System.Net;
using System.Numerics;

namespace LearningCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // new FibonacciMatrix().Test(100_000);
            // PasswordGenerator.Test(100_000, 20);
            // Anagram.Test();
            new MinecraftBedrockServer("xenon.iapetus11.me", 19132).Test();
            // RecamanSequence.Test(100);
        }
    }
}
