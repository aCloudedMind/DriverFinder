using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriverFinder.Core.Models;
using DriverFinder.Core.Interfaces;

namespace DriverFinder.Core.Algorithms
{

        public class BruteForceAlgorithm : IDriverFinderAlgorithm
        {
            public string AlgorithmName => "BruteForce";

            public List<Driver> FindNearestDrivers(Order order, List<Driver> allDrivers, int count = 5)
            {
                return allDrivers
                    .Select(driver => new
                    {
                        Driver = driver,
                        Distance = CalculateDistance(order.X, order.Y, driver.X, driver.Y)
                    })
                    .OrderBy(x => x.Distance)
                    .Take(count)
                    .Select(x => x.Driver)
                    .ToList();
            }

            private double CalculateDistance(int x1, int y1, int x2, int y2)
            {
                return System.Math.Sqrt(System.Math.Pow(x2 - x1, 2) + System.Math.Pow(y2 - y1, 2));
            }
        }
    
}
