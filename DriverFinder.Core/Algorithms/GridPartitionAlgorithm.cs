using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriverFinder.Core.Models;
using DriverFinder.Core.Interfaces;

namespace DriverFinder.Core.Algorithms
{

        public class GridPartitionAlgorithm : IDriverFinderAlgorithm
        {
            private readonly int gridSize;

            public GridPartitionAlgorithm(int gridSize = 10)
            {
                this.gridSize = gridSize;
            }

            public string AlgorithmName => "GridPartition";

            public List<Driver> FindNearestDrivers(Order order, List<Driver> allDrivers, int count = 5)
            {
                var grid = new Dictionary<(int, int), List<Driver>>();

                // Распределяем водителей по ячейкам сетки
                foreach (var driver in allDrivers)
                {
                    var cellKey = (driver.X / gridSize, driver.Y / gridSize);
                    if (!grid.ContainsKey(cellKey))
                    {
                        grid[cellKey] = new List<Driver>();
                    }
                    grid[cellKey].Add(driver);
                }

                // Ищем в соседних ячейках
                var orderCell = (order.X / gridSize, order.Y / gridSize);
                var candidates = new List<Driver>();

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        var neighborCell = (orderCell.Item1 + dx, orderCell.Item2 + dy);
                        if (grid.ContainsKey(neighborCell))
                        {
                            candidates.AddRange(grid[neighborCell]);
                        }
                    }
                }

                // Если в соседних ячейках недостаточно водителей, ищем во всех
                if (candidates.Count < count)
                {
                    candidates = allDrivers.ToList();
                }

                return candidates
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
