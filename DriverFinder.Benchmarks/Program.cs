using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DriverFinder.Core.Models;
using DriverFinder.Core.Algorithms;
using System.Collections.Generic;
using System.Linq;

namespace DriverFinder.Benchmarks
{
    [MemoryDiagnoser]
    public class DriverFinderBenchmarks
    {
        private List<Driver> drivers;
        private Order order;

        [Params(100, 1000, 10000)]
        public int DriverCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var random = new System.Random(42);
            drivers = new List<Driver>();

            for (int i = 0; i < DriverCount; i++)
            {
                drivers.Add(new Driver(
                    i.ToString(),
                    random.Next(0, 1000),
                    random.Next(0, 1000)
                ));
            }

            order = new Order(500, 500);
        }

        [Benchmark]
        public void BruteForceAlgorithm()
            => new BruteForceAlgorithm().FindNearestDrivers(order, drivers, 5);

        [Benchmark]
        public void KDTreeAlgorithm()
            => new KDTreeAlgorithm().FindNearestDrivers(order, drivers, 5);

        [Benchmark]
        public void GridPartitionAlgorithm()
            => new GridPartitionAlgorithm(50).FindNearestDrivers(order, drivers, 5);

        [Benchmark]
        public void PriorityQueueAlgorithm()
            => new PriorityQueueAlgorithm().FindNearestDrivers(order, drivers, 5);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<DriverFinderBenchmarks>();
        }
    }
}