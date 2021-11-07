namespace NSU.Worm
{
    public class Worm
    {
        public Worm(string name, int life, Position position)
        {
            Name = name;
            Life = life;
            Position = position;
        }

        public Worm(string name, int life, int xPosition, int yPosition)
        {
            Name = name;
            Life = life;
            Position = new Position(xPosition, yPosition);
        }

        /// <summary>
        /// Ожидается, что имя червяка будет уникальным в мире
        /// </summary>
        public string Name { get; }
        
        public int Life { get; set; }

        public Position Position { get; set; }

        public override string ToString()
        {
            return $"{Name}-{Life} {Position}";
        }
    }
}