using System;
using System.Collections.Immutable;

namespace NSU.Worm
{
    public interface WorldState
    {
        public ImmutableList<Worm> Worms { get; }

        public ImmutableList<Food> Food { get; }

        [Obsolete("Метод подразумевает, что в одной позиции может находится только один объект, что некорректно." +
                  "Нужно использовать методы IsWorm, IsFood и IsEmpty.")]
        public Tile Get(Position position);

        public bool IsWorm(Position position);

        public bool IsFood(Position position);

        public bool IsEmpty(Position position);

        public void Move(Worm worm, Position position);

        public void Put(Worm worm, Position position);

        public void Remove(Worm worm);

        public void Put(Food food, Position position);

        public void RemoveFood(Position position);

        public string StateToString();

        public string MapToString(int leftBorder = -5, int rightBorder = 5, int topBorder = 5, int bottomBorder = -5);

        public enum Tile : byte
        {
            Empty = 0,
            Worm = 1,
            Food = 2
        }
    }
}