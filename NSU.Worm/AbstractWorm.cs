﻿namespace NSU.Worm
{
    public abstract class AbstractWorm : Worm
    {
        protected AbstractWorm(string name, int xPosition, int yPosition)
        {
            Name = name;
            Position = new Position(xPosition, yPosition);
        }

        public string Name { get; }
        
        public Position Position { get; set; }

        public abstract WormAction GetAction();

        public override string ToString()
        {
            return $"{Name} {Position}";
        }
    }
}