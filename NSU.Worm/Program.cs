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
                new Worm("Sasha", 10, 10, 0, new CirclingWormAI(10, 0)),
                new Worm("Zhenya", 5, 10, 1, new CirclingWormAI(10, 1))
            };

            var simulator = new Simulator(worms);
            simulator.Start(Iterations);
        }
    }
}