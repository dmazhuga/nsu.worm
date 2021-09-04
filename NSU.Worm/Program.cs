using System.Collections.Generic;

namespace NSU.Worm
{
    class Program
    {
        static void Main(string[] args)
        {
            var worms = new List<Worm>();
            worms.Add(new CirclingWorm("Sasha", 0, 0));

            var simulator = new Simulator(worms);
            simulator.Start();
        }
    }
}