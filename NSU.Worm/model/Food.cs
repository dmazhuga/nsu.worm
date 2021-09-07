namespace NSU.Worm
{
    public class Food
    {
        public Food(Position position)
        {
            Position = position;
        }

        public Food(int xPosition, int yPosition)
        {
            Position = new Position(xPosition, yPosition);
        }

        public Position Position { get; set; }
    }
}