using DriverFinder.Core.Models;
using DriverFinder.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DriverFinder.Core.Algorithms
{
    public class PriorityQueueAlgorithm : IDriverFinderAlgorithm
    {
        public string AlgorithmName => "PriorityQueue";

        public List<Driver> FindNearestDrivers(Order order, List<Driver> allDrivers, int count = 5)
        {
            var priorityQueue = new SortedList<double, Driver>(new DuplicateKeyComparer<double>());

            foreach (var driver in allDrivers)
            {
                double distance = CalculateDistance(order.X, order.Y, driver.X, driver.Y);
                priorityQueue.Add(distance, driver);
            }

            return priorityQueue.Values.Take(count).ToList();
        }

        private double CalculateDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);
                return result == 0 ? 1 : result;
            }
        }
    }
}