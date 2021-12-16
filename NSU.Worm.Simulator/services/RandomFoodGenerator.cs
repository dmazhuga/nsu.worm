using System;

namespace NSU.Worm
{
    public class RandomFoodGenerator : IFoodGenerator
    {
        private Random _random;

        public RandomFoodGenerator()
        {
            _random = new Random();
        }

        public Food GenerateFood(int freshness)
        {
            var x = _random.NextNormal();
            var y = _random.NextNormal();
            
            return new Food(x, y, freshness);
        }
    }
}