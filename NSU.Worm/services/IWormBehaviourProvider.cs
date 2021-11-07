namespace NSU.Worm
{
    public interface IWormBehaviourProvider
    {
        public IWormBehaviour GetBehaviour(Worm worm);

        public void RegisterBehaviour(Worm worm, IWormBehaviour behaviour);

        public void DeleteBehaviour(Worm worm);
    }
}