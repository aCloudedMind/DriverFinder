﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverFinder.Core.Models
{
    public class Order
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Order(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
