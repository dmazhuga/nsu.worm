using System.Collections.Generic;

namespace NSU.Worm
{
    class Program
    {
        private const int Iterations = 10;
        public static void Main(string[] args)
        {
            var worms = new List<Worm>
            {
                new Worm("Sasha", 10, 2, 0, new CirclingWormAI(2, 0)),
                new Worm("Zhenya", 10, -2, 0, new CirclingWormAI(-2, 0))
            };

            var simulator = new Simulator(worms);
            simulator.Start(Iterations);
        }
    }
}