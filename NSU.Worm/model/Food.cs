namespace NSU.Worm
{
    public class Food
    {
        public Food(Position position, int freshness = 10)
        {
            Position = position;
            Freshness = freshness;
        }

        public Food(int xPosition, int yPosition, int freshness = 10)
        {
            Freshness = freshness;
            Position = new Position(xPosition, yPosition);
        }

        public Position Position { get; set; }
        
        public int Freshness { get; set; }
    }
}