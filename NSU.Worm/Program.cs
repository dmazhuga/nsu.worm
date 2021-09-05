using System.Collections.Generic;

namespace NSU.Worm
{
    class Program
    {
        private const int Iterations = 10;
        static void Main(string[] args)
        {
            var worms = new List<Worm>
            {
                new CirclingWorm("Sasha", 10, 0),
                new CirclingWorm("Zhenya", 10, 1)
            };

            var simulator = new Simulator(worms);
            simulator.Start(Iterations);
        }
    }
}