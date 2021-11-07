using System;
using System.ComponentModel;

namespace NSU.Worm
{
    public class Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }

        public Position Next(Direction direction)
        {
            return direction switch
            {
                Direction.UpLeft => new Position(X - 1, Y + 1),
                Direction.Up => new Position(X, Y + 1),
                Direction.UpRight => new Position(X + 1, Y + 1),

                Direction.Left => new Position(X - 1, Y),
                Direction.Right => new Position(X + 1, Y),

                Direction.DownLeft => new Position(X - 1, Y - 1),
                Direction.Down => new Position(X, Y - 1),
                Direction.DownRight => new Position(X + 1, Y - 1),

                Direction.None => this,

                _ => throw new InvalidEnumArgumentException($"Unsupported direction argument: {direction}")
            };
        }

        public Direction DirectionRelativeTo(Position other)
        {
            foreach (var direction in (Direction[]) Enum.GetValues(typeof(Direction)))
            {
                if (this == other.Next(direction))
                {
                    return direction;
                }
            }

            return Direction.None;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            var target = (Position) obj;
            return (X == target.X) && (Y == target.Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Position p1, Position p2)
        {
            if (p1 is not null) return p1.Equals(p2);

            return p2 is null;
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return !(p1 == p2);
        }
    }
}