namespace NSU.Worm
{
    public interface Worm
    {
        public string Name { get; }

        public Position Position { get; set; }

        public WormAction GetAction();
    }
}