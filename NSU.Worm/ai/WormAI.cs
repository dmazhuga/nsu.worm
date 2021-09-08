namespace NSU.Worm
{
    public interface WormAI
    {
        public WormAction GetNextAction(Position position, int life);
    }
}