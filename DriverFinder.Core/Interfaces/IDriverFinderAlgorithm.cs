using DriverFinder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverFinder.Core.Interfaces
{

   
        public interface IDriverFinderAlgorithm
        {
            List<Driver> FindNearestDrivers(Order order, List<Driver> allDrivers, int count = 5);
            string AlgorithmName { get; }
        }
    
}
