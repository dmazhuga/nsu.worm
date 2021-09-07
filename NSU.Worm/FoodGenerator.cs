﻿using System;

namespace NSU.Worm
{
    public class FoodGenerator
    {
        private Random _random;

        public FoodGenerator()
        {
            _random = new Random();
        }

        public Food GenerateFood()
        {
            var x = _random.NextNormal();
            var y = _random.NextNormal();
            
            return new Food(x, y);
        }
    }
}