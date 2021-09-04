namespace NSU.Worm
{
    public class Worm
    {
        public Worm(string name, int xPosition, int yPosition)
        {
            Name = name;
            Position = new Position(xPosition, yPosition);
        }

        public string Name { get; }
        
        public Position Position { get; set; }

        private WormAction _lastMove;

        public WormAction GetAction()
        {
            return WormAction.MoveUp;
        }

        public override string ToString()
        {
            return $"{Name} {Position}";
        }
    }
}