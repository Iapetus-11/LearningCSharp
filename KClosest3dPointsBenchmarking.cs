using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace LearningCSharp;

public class KClosest3dPointsBenchmarking
{
    private Vector3[] _points;
    private Vector3[] _pointsCopy = null!;

    [Params(1, 5, 10, 100, 1000, 10_000, 100_000)]
    public int K;

    public KClosest3dPointsBenchmarking()
    {
        _points = Array.Empty<Vector3>();
    }
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        var rand = new Random();

        _points = Enumerable.Range(0, K * 10)
            .Select(_ => new Vector3(rand.NextSingle() * K * 10, rand.NextSingle() * K * 10, rand.NextSingle() * K * 10))
            .ToArray();
        
        Console.WriteLine(_points.Length);
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _pointsCopy = new Vector3[_points.Length];
        Array.Copy(_points, _pointsCopy, _points.Length);
    }

    [Benchmark]
    public Vector3[] GetClosestPointsLinq()
    {
        return _points.OrderBy(v => Vector3.Distance(v, Vector3.Zero)).Take(K).ToArray();
    }

    [Benchmark]
    public Vector3[] GetClosestPointsArray()
    {
        var pointsDistances = new float[_points.Length];

        for (var i = 0; i < _points.Length; i++)
        {
            pointsDistances[i] = Vector3.Distance(_points[i], Vector3.Zero);
        }
        
        Array.Sort(pointsDistances, _pointsCopy);

        return _points[..K];
    }

    [Benchmark]
    public Vector3[] GetClosestPointsPriorityQueue()
    {
        var q = new PriorityQueue<Vector3, float>();
        
        foreach (var p in _points)
        {
            if (q.Count < K)
            {
                q.Enqueue(p, float.MaxValue - Vector3.Distance(p, Vector3.Zero));
            }
            else
            {
                q.Enqueue(p, float.MaxValue - Vector3.Distance(p, Vector3.Zero));
                q.Dequeue();
            }
        }

        return Enumerable.Range(1, K).Select(_ => q.Dequeue()).ToArray();
    }

    public static void Test()
    {
        var x = new KClosest3dPointsBenchmarking();
        x.K = 2;
        x.GlobalSetup();
        x.IterationSetup();
        
        Console.WriteLine($"GetClosestPointsLinq: [{string.Join(", ", x.GetClosestPointsLinq())}]");
        Console.WriteLine($"GetClosestPointsArray: [{string.Join(", ", x.GetClosestPointsArray())}]");
        Console.WriteLine($"GetClosestPointsPriorityQueue: [{string.Join(", ", x.GetClosestPointsPriorityQueue())}]");
    }
}