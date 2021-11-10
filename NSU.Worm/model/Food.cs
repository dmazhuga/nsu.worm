namespace NSU.Worm
{
    public class Food
    {
        public Food(Position position, int freshness)
        {
            Position = position;
            Freshness = freshness;
        }

        public Food(int xPosition, int yPosition, int freshness )
        {
            Freshness = freshness;
            Position = new Position(xPosition, yPosition);
        }

        public Position Position { get; set; }
        
        public int Freshness { get; set; }
    }
}