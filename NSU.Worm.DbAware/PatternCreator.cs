using System;
using System.Collections.Generic;
using NSU.Worm.Data;

namespace NSU.Worm.CreateFoodPattern
{
    public class PatternCreator
    {
        private IFoodGenerator _randomFoodGenerator;

        public PatternCreator(IFoodGenerator randomFoodGenerator)
        {
            _randomFoodGenerator = randomFoodGenerator;
        }

        public void CreateAndSaveRandom(string name, int iterations, EnvironmentContext context)
        {
            var steps = new List<FoodGenerationPatternStep>();

            for (var i = 0; i < iterations; i++)
            {
                var position = _randomFoodGenerator.GenerateFood(default).Position;

                var step = new FoodGenerationPatternStep
                    {Iteration = i, PositionX = position.X, PositionY = position.Y};

                steps.Add(step);
            }

            var pattern = new FoodGenerationPattern {Name = name, Iterations = iterations, Steps = steps};

            context.Patterns.Add(pattern);
            context.SaveChanges();
        }

        public void CreateAndSaveFromPositionList(string name, int iterations, List<Position> positions, EnvironmentContext context)
        {
            if (positions.Count != iterations)
            {
                throw new ArgumentException("Wrong number of positions provided");
            }
            
            var steps = new List<FoodGenerationPatternStep>();

            for (var i = 0; i < iterations; i++)
            {
                var position = positions[i];

                var step = new FoodGenerationPatternStep
                    {Iteration = i, PositionX = position.X, PositionY = position.Y};

                steps.Add(step);
            }

            var pattern = new FoodGenerationPattern {Name = name, Iterations = iterations, Steps = steps};

            context.Patterns.Add(pattern);
            context.SaveChanges();
        }
    }
}