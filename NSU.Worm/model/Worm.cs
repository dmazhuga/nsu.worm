namespace NSU.Worm
{
    public class Worm
    {
        private readonly WormAI _ai;
        
        public Worm(string name, int life, Position position, WormAI ai)
        {
            Name = name;
            Life = life;
            Position = position;

            _ai = ai;
        }

        public Worm(string name, int life, int xPosition, int yPosition, WormAI ai)
        {
            Name = name;
            Life = life;
            Position = new Position(xPosition, yPosition);

            _ai = ai;
        }

        public string Name { get; }
        
        public int Life { get; set; }

        public Position Position { get; set; }

        public WormAction GetAction()
        {
            return _ai.GetNextAction(Position, Life);
        }

        public Worm Reproduce(string childName, Position position, int childLife)
        {
            return new Worm(childName, childLife, position, _ai);
        }

        public override string ToString()
        {
            return $"{Name}-{Life} {Position}";
        }
    }
}