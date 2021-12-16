using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSU.Worm.Data;

namespace NSU.Worm
{
    public class PatternFoodGenerator : IFoodGenerator
    {
        private readonly FoodGenerationPattern _pattern;

        private int _iteration;
        
        public PatternFoodGenerator(IOptions<SimulatorOptions> options, IDbContextFactory<EnvironmentContext> contextFactory)
        {
            using var context = contextFactory.CreateDbContext();

            var patternName = options.Value.FoodPatternName;

            _pattern = context.Patterns
                .Where(p => p.Name.Equals(patternName))
                .Include(p => p.Steps)
                .Single();

            _iteration = 0;
        }

        public Food GenerateFood(int freshness)
        {
            var step = _pattern.Steps.Single(s => s.Iteration == _iteration);
            _iteration++;
            return new Food(step.PositionX, step.PositionY, freshness);
        }
    }
}