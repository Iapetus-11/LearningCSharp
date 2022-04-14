﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Numerics;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;

namespace LearningCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // new FibonacciMatrix().Test(100_000);
            // PasswordGenerator.Test(100_000, 20);
            // Anagram.Test();
            // new MinecraftBedrockServer("xenon.iapetus11.me", 19132).Test();
            // RecamanSequence.Test(100);
            
            // for testing the circular list
            // var circleList = new CircularList<int>();
            // for (int i = 0; i < 5; i++) circleList.Add(i);
            // for (int i = 0; i < circleList.Count * 2; i++) Console.WriteLine(circleList[i]);
            // circleList.Insert(1, 1);
            // circleList.Insert(circleList.Count + 2, 1);
            // for (int i = 0; i < circleList.Count; i++) Console.WriteLine(circleList[i]);

            // MinecraftServerRCON.Test();
            
            // FakeHashCode.Test();
            
            // var config = new ManualConfig()
            //         .WithOptions(ConfigOptions.DisableOptimizationsValidator)
            //         .AddValidator(JitOptimizationsValidator.DontFailOnError)
            //         .AddLogger(ConsoleLogger.Default)
            //         .AddColumnProvider(DefaultColumnProviders.Instance);
            //
            // BenchmarkRunner.Run<KClosest3dPointsBenchmarking>(config);
            
            KClosest3dPointsBenchmarking.Test();
        }
    }
}
