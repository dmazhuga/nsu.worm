using System;

namespace NSU.Worm
{
    public class FoodGenerator : IFoodGenerator
    {
        private Random _random;

        public FoodGenerator()
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