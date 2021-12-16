using System.Collections.Generic;

namespace NSU.Worm
{
    public class SimulatorOptions
    {
        public int Iterations { get; set; } = 100;

        public int WormLifeStart { get; set; } = 10;

        public int WormLifeBorderline { get; set; } = 0;

        public int FoodFreshnessStart { get; set; } = 10;

        public int FoodFreshnessBorderline { get; set; } = 0;

        public int FoodLifeGain { get; set; } = 10;

        public int ReproductionLifeCost { get; set; } = 10;

        public int ReproductionLifeRequirement { get; set; } = 10;

        public List<Worm> StartWorms { get; set; } = new();

        public List<string> NamePool { get; set; } = new();

        public string FoodPatternName { get; set; } = "";
    }
}