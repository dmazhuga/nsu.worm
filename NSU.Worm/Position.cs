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

        public Position NextLeft()
        {
            return new Position(X - 1, Y);
        }
        
        public Position NextRight()
        {
            return new Position(X + 1, Y);
        }
        
        public Position NextDown()
        {
            return new Position(X, Y - 1);
        }
        
        public Position NextUp()
        {
            return new Position(X, Y + 1);
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