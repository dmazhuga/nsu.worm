using System.Collections.Generic;

namespace NSU.Worm
{
    class Program
    {
        private const int Iterations = 100;

        public static void Main(string[] args)
        {
            var worm1 = new Worm("Sasha", 10, 2, 0);
            var behaviour1 = new CirclingWormBehaviour(worm1, worm1.Position);

            var worm2 = new Worm("Zhenya", 10, -2, 0);
            var behaviour2 = new CirclingWormBehaviour(worm2, worm2.Position);

            var worms = new List<KeyValuePair<Worm, IWormBehaviour>>
            {
                new(worm1, behaviour1),
                new(worm2, behaviour2)
            };

            var simulator = new Simulator(worms);
            simulator.Start(Iterations);
        }
    }
}