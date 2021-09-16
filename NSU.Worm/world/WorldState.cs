using System;
using System.Collections.Immutable;

namespace NSU.Worm
{
    public interface WorldState
    {
        public ImmutableList<Worm> Worms { get; }

        public ImmutableList<Food> Food { get; }
        
        public Tile Get(Position position);

        public Worm GetWorm(Position position);

        public Food GetFood(Position position);

        public void Move(Worm worm, Position position);

        public void Put(Worm worm, Position position);

        public void Remove(Worm worm);

        public void Put(Food food, Position position);
        
        public void Remove(Food food);

        public string StateToString();

        public string MapToString(int leftBorder = -5, int rightBorder = 5, int topBorder = 5, int bottomBorder = -5);

        public enum Tile : byte
        {
            Empty = 0,
            Worm = 1,
            Food = 2
        }
    }
}