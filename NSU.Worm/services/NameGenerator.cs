using System;
using System.Collections.Generic;

namespace NSU.Worm
{
    public class NameGenerator : INameGenerator
    {
        private Random _random;

        public List<string> NamePool { get; set; }

        public NameGenerator()
        {
            _random = new Random();
        }

        public string NextName()
        {
            if (NamePool.Count == 0)
            {
                throw new ArgumentException("Run out of names!");
            }
            
            var randomIndex = _random.Next(0, NamePool.Count);
            var newName = NamePool[randomIndex];

            NamePool.Remove(newName);

            return newName;
        }
    }
}