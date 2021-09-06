namespace NSU.Worm
{
    public class Worm
    {
        private readonly WormAI _ai;

        public Worm(string name, int xPosition, int yPosition, WormAI ai)
        {
            Name = name;
            Position = new Position(xPosition, yPosition);

            _ai = ai;
        }

        public string Name { get; }

        public Position Position { get; set; }

        public WormAction GetAction()
        {
            return _ai.GetNextAction(Position);
        }

        public override string ToString()
        {
            return $"{Name} {Position}";
        }
    }
}