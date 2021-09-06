using System.Collections.Immutable;

namespace NSU.Worm
{
    public interface WorldState
    {
        public ImmutableList<Worm> Worms { get; }

        public void Move(Worm worm, Position position);

        public void Put(Worm worm, Position position);

        public void Remove(Worm worm);

        public Tile Get(Position position);

        public string StateToString();

        public string MapToString(int leftBorder = -5, int rightBorder = 5, int topBorder = 5, int bottomBorder = -5);

        public enum Tile : byte
        {
            Empty = 0,
            Worm = 1
        }
    }
}