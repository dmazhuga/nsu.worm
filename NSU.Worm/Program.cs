using System.Collections.Generic;

namespace NSU.Worm
{
    class Program
    {
        static void Main(string[] args)
        {
            var worms = new List<Worm>();
            worms.Add(new Worm("Sasha", 0, 0));
            worms.Add(new Worm("Zhenya", 0, 1));
            
            var simulator = new Simulator(worms);
            simulator.Start();
        }
    }
}