using System.Collections.Generic;

namespace NSU.Worm.Data
{
    public class FoodGenerationPattern
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Iterations { get; set; }

        public IList<FoodGenerationPatternStep> Steps { get; set; }
    }
}