using System;
using BenchmarkDotNet.Running;

namespace LinearAlgebra.Performance
{
    class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<VectorPerformanceTests>();
        }
    }
}