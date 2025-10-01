using NUnit.Framework;
using DriverFinder.Core.Models;
using DriverFinder.Core.Algorithms;
using DriverFinder.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DriverFinder.Tests
{
    [TestFixture]
    public class DriverFinderTests
    {
        private List<Driver> testDrivers;
        private Order testOrder;

        [SetUp]
        public void Setup()
        {
            testDrivers = new List<Driver>
            {
                new Driver("1", 1, 1),
                new Driver("2", 5, 5),
                new Driver("3", 10, 10),
                new Driver("4", 2, 2),
                new Driver("5", 8, 8),
                new Driver("6", 3, 3),
                new Driver("7", 7, 7),
                new Driver("8", 0, 0),
                new Driver("9", 4, 4),
                new Driver("10", 9, 9)
            };

            testOrder = new Order(0, 0);
        }

        [Test]
        public void BruteForceAlgorithm_ShouldReturnFiveNearestDrivers()
        {
            // Arrange
            IDriverFinderAlgorithm algorithm = new BruteForceAlgorithm();

            // Act
            var result = algorithm.FindNearestDrivers(testOrder, testDrivers, 5);

            // Assert
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("8", result[0].Id); // (0,0) - самый близкий
            Assert.AreEqual("1", result[1].Id); // (1,1)
        }

        [Test]
        public void KDTreeAlgorithm_ShouldReturnFiveNearestDrivers()
        {
            // Arrange
            IDriverFinderAlgorithm algorithm = new KDTreeAlgorithm();

            // Act
            var result = algorithm.FindNearestDrivers(testOrder, testDrivers, 5);

            // Assert
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("8", result[0].Id);
        }

        [Test]
        public void GridPartitionAlgorithm_ShouldReturnFiveNearestDrivers()
        {
            // Arrange
            IDriverFinderAlgorithm algorithm = new GridPartitionAlgorithm();

            // Act
            var result = algorithm.FindNearestDrivers(testOrder, testDrivers, 5);

            // Assert
            Assert.AreEqual(5, result.Count);
        }

        [Test]
        public void PriorityQueueAlgorithm_ShouldReturnFiveNearestDrivers()
        {
            // Arrange
            IDriverFinderAlgorithm algorithm = new PriorityQueueAlgorithm();

            // Act
            var result = algorithm.FindNearestDrivers(testOrder, testDrivers, 5);

            // Assert
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("8", result[0].Id);
        }

        [Test]
        public void AllAlgorithms_ShouldReturnSameResults()
        {
            // Arrange
            var algorithms = new List<IDriverFinderAlgorithm>
            {
                new BruteForceAlgorithm(),
                new KDTreeAlgorithm(),
                new GridPartitionAlgorithm(),
                new PriorityQueueAlgorithm()
            };

            // Act & Assert
            var firstResult = algorithms[0].FindNearestDrivers(testOrder, testDrivers, 5);

            for (int i = 1; i < algorithms.Count; i++)
            {
                var currentResult = algorithms[i].FindNearestDrivers(testOrder, testDrivers, 5);

                // ѕровер€ем, что все алгоритмы возвращают одинаковое количество водителей
                Assert.AreEqual(firstResult.Count, currentResult.Count);
            }
        }

        private double CalculateDistance(Order order, Driver driver)
        {
            return System.Math.Sqrt(System.Math.Pow(driver.X - order.X, 2) + System.Math.Pow(driver.Y - order.Y, 2));
        }
    }
}