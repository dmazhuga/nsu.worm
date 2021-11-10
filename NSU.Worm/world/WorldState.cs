using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace NSU.Worm
{
    public class WorldState : IMutableWorldState
    {
        private readonly List<Worm> _worms;

        private readonly List<Food> _food;

        public WorldState()
        {
            _worms = new List<Worm>();
            _food = new List<Food>();
        }

        public WorldState(List<Worm> wormList)
        {
            _worms = new List<Worm>();
            _food = new List<Food>();

            foreach (var worm in wormList)
            {
                Put(worm);
            }
        }

        public WorldState(List<Worm> wormList, List<Food> foodList)
        {
            _worms = new List<Worm>();
            _food = new List<Food>();

            foreach (var worm in wormList)
            {
                Put(worm);
            }

            foreach (var food in foodList)
            {
                Put(food);
            }
        }

        public ImmutableList<Worm> Worms => _worms.ToImmutableList();

        public ImmutableList<Food> Food => _food.ToImmutableList();

        public virtual void Move(Worm worm, Position position)
        {
            if (worm.Position == position)
            {
                return;
            }

            if (!Exists(worm))
            {
                throw new ArgumentException(
                    "No such worm in current world state");
            }

            if (Get(position) != Tile.Empty)
            {
                throw new ArgumentException(
                    $"Cannot move worm to position {position} - it is not empty");
            }

            worm.Position = position;
        }

        public virtual void Put(Worm worm)
        {
            Put(worm, worm.Position);
        }

        public virtual void Put(Worm worm, Position position)
        {
            if (Exists(worm))
            {
                throw new ArgumentException(
                    "This worm already exists in current world state");
            }

            if (GetWorm(worm.Name) is not null)
            {
                throw new ArgumentException(
                    $"Worm with name {worm.Name} already exists. Name should be unique.");
            }

            if (Get(position) != Tile.Empty)
            {
                throw new ArgumentException(
                    $"Cannot put worm to position {position} - it is not empty");
            }

            worm.Position = position;
            _worms.Add(worm);
        }

        public virtual void Remove(Worm worm)
        {
            if (!Exists(worm))
            {
                throw new ArgumentException(
                    "No such worm in current world state");
            }

            _worms.Remove(worm);
        }

        public virtual void Put(Food food)
        {
            Put(food, food.Position);
        }

        public virtual void Put(Food food, Position position)
        {
            if (_food.Contains(food))
            {
                throw new ArgumentException(
                    "This food already exists in current world state");
            }

            if (Get(position) != Tile.Empty)
            {
                throw new ArgumentException(
                    $"Cannot put food to position {position} - it is not empty");
            }

            food.Position = position;
            _food.Add(food);
        }

        public virtual void Remove(Food food)
        {
            if (!Exists(food))
            {
                throw new ArgumentException(
                    "No such food in current world state");
            }

            _food.Remove(food);
        }

        public virtual Tile Get(Position position)
        {
            if (_worms.Any(worm => worm.Position == position))
            {
                return Tile.Worm;
            }

            if (_food.Any(food => food.Position == position))
            {
                return Tile.Food;
            }

            return Tile.Empty;
        }

        public Worm GetWorm(string name)
        {
            return _worms.Find(worm => worm.Name == name);
        }

        public virtual Worm GetWorm(Position position)
        {
            return _worms.Find(worm => worm.Position == position);
        }

        public virtual Food GetFood(Position position)
        {
            return _food.Find(food => food.Position == position);
        }

        protected bool Exists(Worm worm)
        {
            return _worms.Contains(worm);
        }

        protected bool Exists(Food food)
        {
            return _food.Contains(food);
        }
    }
}