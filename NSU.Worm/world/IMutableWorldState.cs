namespace NSU.Worm
{
    public interface IMutableWorldState : IWorldState
    {
        public void Move(Worm worm, Position position);

        public void Put(Worm worm, Position position);

        public void Remove(Worm worm);

        public void Put(Food food, Position position);

        public void Remove(Food food);
    }
}