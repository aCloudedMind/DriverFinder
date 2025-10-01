using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriverFinder.Core.Models;
using DriverFinder.Core.Interfaces;

namespace DriverFinder.Core.Algorithms
{

        public class KDTreeAlgorithm : IDriverFinderAlgorithm
        {
            public string AlgorithmName => "KDTree";

            public List<Driver> FindNearestDrivers(Order order, List<Driver> allDrivers, int count = 5)
            {
                if (allDrivers.Count <= count)
                    return allDrivers.ToList();

                var kdTree = new KDTree(allDrivers);
                return kdTree.FindNearestNeighbors(order.X, order.Y, count);
            }

            private class KDTree
            {
                private class Node
                {
                    public Driver Driver { get; set; }
                    public Node Left { get; set; }
                    public Node Right { get; set; }

                    public Node(Driver driver)
                    {
                        Driver = driver;
                    }
                }

                private Node root;

                public KDTree(List<Driver> drivers)
                {
                    root = BuildTree(drivers, 0);
                }

                private Node BuildTree(List<Driver> drivers, int depth)
                {
                    if (drivers.Count == 0) return null;

                    int axis = depth % 2;
                    var sorted = axis == 0
                        ? drivers.OrderBy(d => d.X).ToList()
                        : drivers.OrderBy(d => d.Y).ToList();

                    int mid = sorted.Count / 2;
                    var node = new Node(sorted[mid]);

                    node.Left = BuildTree(sorted.Take(mid).ToList(), depth + 1);
                    node.Right = BuildTree(sorted.Skip(mid + 1).ToList(), depth + 1);

                    return node;
                }

                public List<Driver> FindNearestNeighbors(int x, int y, int count)
                {
                    var neighbors = new List<(Driver driver, double distance)>();
                    FindNearest(root, x, y, count, 0, neighbors);
                    return neighbors.OrderBy(n => n.distance).Take(count).Select(n => n.driver).ToList();
                }

                private void FindNearest(Node node, int x, int y, int count, int depth,
                                       List<(Driver driver, double distance)> neighbors)
                {
                    if (node == null) return;

                    double distance = CalculateDistance(x, y, node.Driver.X, node.Driver.Y);
                    neighbors.Add((node.Driver, distance));

                    // Поддерживаем только count ближайших
                    if (neighbors.Count > count * 2)
                    {
                        neighbors = neighbors.OrderBy(n => n.distance).Take(count).ToList();
                    }

                    int axis = depth % 2;
                    Node nextBranch, otherBranch;

                    if ((axis == 0 && x < node.Driver.X) || (axis == 1 && y < node.Driver.Y))
                    {
                        nextBranch = node.Left;
                        otherBranch = node.Right;
                    }
                    else
                    {
                        nextBranch = node.Right;
                        otherBranch = node.Left;
                    }

                    FindNearest(nextBranch, x, y, count, depth + 1, neighbors);

                    // Проверяем другуюветвь
                    double axisDistance = axis == 0
                        ? System.Math.Abs(x - node.Driver.X)
                        : System.Math.Abs(y - node.Driver.Y);

                    if (neighbors.Count < count || axisDistance < neighbors.Last().distance)
                    {
                        FindNearest(otherBranch, x, y, count, depth + 1, neighbors);
                    }
                }

                private double CalculateDistance(int x1, int y1, int x2, int y2)
                {
                    return System.Math.Sqrt(System.Math.Pow(x2 - x1, 2) + System.Math.Pow(y2 - y1, 2));
                }
            }
        }
    
}
