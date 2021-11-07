using System;
using System.Collections.Immutable;
using System.Text;

namespace NSU.Worm
{
    public interface IWorldState
    {
        public ImmutableList<Worm> Worms { get; }

        public ImmutableList<Food> Food { get; }

        public Tile Get(Position position);

        public Worm GetWorm(string name);

        public Worm GetWorm(Position position);

        public Food GetFood(Position position);

        public string StateToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Worms: [");

            foreach (var worm in Worms)
            {
                stringBuilder.Append($"{worm}, ");
            }

            if (Worms.Count != 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }

            stringBuilder.Append(']');

            stringBuilder.Append(", Food: [");

            foreach (var food in Food)
            {
                stringBuilder.Append($"{food.Position}, ");
            }

            if (Food.Count != 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }

            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }

        public string MapToString(int leftBorder = -5, int rightBorder = 5, int topBorder = 5, int bottomBorder = -5)
        {
            throw new NotImplementedException(); //TODO
        }
    }

    public enum Tile : byte
    {
        Empty = 0,
        Worm = 1,
        Food = 2
    }
}