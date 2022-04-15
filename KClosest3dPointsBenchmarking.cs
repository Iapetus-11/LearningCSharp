using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace LearningCSharp;

[SimpleJob(id: "ID_0"), RPlotExporter]
public class KClosest3dPointsBenchmarking
{
    private const float _maxCoordSize = 1000f;
    private Vector3[] _pointsOg = null!;
    private Vector3[] _points = null!;

    [Params(1, 100, 1000)]
    public int KClosest;

    [Params(100_000, 1_000_000)]
    public int NPoints;
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        var rand = new Random();

        _pointsOg = Enumerable.Range(0, NPoints)
            .Select(_ => new Vector3(rand.NextSingle() * _maxCoordSize, rand.NextSingle() * _maxCoordSize,
                rand.NextSingle() * _maxCoordSize))
            .ToArray();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        // GetClosestPointsArray sorts _points in place
        // so to avoid skewing results we have to recreate it every iteration
        _points = new Vector3[_pointsOg.Length];
        Array.Copy(_pointsOg, _points, _pointsOg.Length);
    }

    [Benchmark(Description = "Array", OperationsPerInvoke = 1048576)]
    public Vector3[] GetClosestPointsArray()
    {
        var pointLengths = new float[_points.Length];

        for (var i = 0; i < _points.Length; i++)
        {
            pointLengths[i] = _points[i].Length();
        }
        
        Array.Sort(pointLengths, _points);

        return _points[..KClosest];
    }

    [Benchmark(Description = "LINQ", OperationsPerInvoke = 1048576)]
    public Vector3[] GetClosestPointsLinq()
    {
        return _points.OrderBy(v => v.Length()).Take(KClosest).ToArray();
    }
    
    [Benchmark(Description = "PriorityQueue", OperationsPerInvoke = 1048576)]
    public Vector3[] GetClosestPointsPriorityQueue()
    {
        var q = new PriorityQueue<Vector3, float>(KClosest);
        
        foreach (var p in _points)
        {
            if (q.Count < KClosest)
            {
                q.Enqueue(p, _maxCoordSize - p.Length());
            }
            else
            {
                q.EnqueueDequeue(p, _maxCoordSize - p.Length());
            }
        }

        // convert the result to an array
        Span<Vector3> result = stackalloc Vector3[KClosest];

        for (var i = KClosest-1; i >= 0; i--)
        {
            result[i] = q.Dequeue();
        }

        return result.ToArray();
    }

    public static void Test()
    {
        var x = new KClosest3dPointsBenchmarking
        {
            // KClosest = 1000,
            KClosest = 2,
            NPoints = 1_000_000
        };
        
        x.GlobalSetup();
        
        x.IterationSetup(); Console.WriteLine($"GetClosestPointsLinq: [{string.Join(", ", x.GetClosestPointsLinq())}]");
        x.IterationSetup(); Console.WriteLine($"GetClosestPointsPriorityQueue: [{string.Join(", ", x.GetClosestPointsPriorityQueue())}]");
        x.IterationSetup(); Console.WriteLine($"GetClosestPointsArray: [{string.Join(", ", x.GetClosestPointsArray())}]");
    }

    public static void Benchmark()
    {
        var config =
            DefaultConfig.Instance
                .WithArtifactsPath(@"C:\Users\miloi\Projects\LearningCSharp\BenchmarkDotNet.Artifacts");

        BenchmarkRunner.Run<KClosest3dPointsBenchmarking>(config);
    }
}