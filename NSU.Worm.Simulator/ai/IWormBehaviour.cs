namespace NSU.Worm
{
    public interface IWormBehaviour
    {
        public string WormName { get; }
        public WormAction GetAction(IWorldState worldState);

        public IWormBehaviour CopyForWorm(Worm worm);
    }
}