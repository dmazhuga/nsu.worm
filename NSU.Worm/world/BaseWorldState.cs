using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace NSU.Worm
{
    public class BaseWorldState : WorldState
    {
        private readonly List<Worm> _worms;

        private readonly List<Food> _food;

        public BaseWorldState(List<Worm> worms)
        {
            _worms = new List<Worm>();
            _food = new List<Food>();

            foreach (var worm in worms)
            {
                Put(worm, worm.Position);
            }
        }

        public ImmutableList<Worm> Worms => _worms.ToImmutableList();

        public ImmutableList<Food> Food => _food.ToImmutableList();

        public virtual void Move(Worm worm, Position position)
        {
            if (!Exists(worm))
            {
                throw new ArgumentException(
                    "No such worm in current world state");
            }

            if (Get(position) == WorldState.Tile.Worm)
            {
                throw new ArgumentException(
                    $"Cannot move worm to position {position} - it is used by other worm");
            }

            worm.Position = position;
        }

        public virtual void Put(Worm worm, Position position)
        {
            if (Exists(worm))
            {
                throw new ArgumentException(
                    "This worm already exists in current world state");
            }

            if (Get(position) == WorldState.Tile.Worm)
            {
                throw new ArgumentException(
                    $"Cannot put worm to position {position} - it is used by other worm");
            }

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

        public virtual void Put(Food food, Position position)
        {
            if (_food.Contains(food))
            {
                throw new ArgumentException(
                    "This food already exists in current world state");
            }

            if (Get(position) == WorldState.Tile.Food)
            {
                throw new ArgumentException(
                    $"Cannot put food to position {position} - it is used by other food");
            }

            _food.Add(food);
        }

        public virtual void RemoveFood(Position position)
        {
            var food = _food.Find(food => food.Position == position);
            
            if (food is null)
            {
                throw new ArgumentException(
                    "No such food in current world state");
            }

            _food.Remove(food);
        }

        public virtual WorldState.Tile Get(Position position)
        {
            if (_worms.Any(worm => worm.Position == position))
            {
                return WorldState.Tile.Worm;
            }

            if (_food.Any(food => food.Position == position))
            {
                return WorldState.Tile.Food;
            }

            return WorldState.Tile.Empty;
        }

        public virtual bool IsWorm(Position position)
        {
            return _worms.Any(worm => worm.Position == position);
        }

        public virtual bool IsFood(Position position)
        {
            return _food.Any(food => food.Position == position);
        }

        public virtual bool IsEmpty(Position position)
        {
            return IsWorm(position) || IsFood(position);
        }

        public virtual string StateToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Worms: [");

            foreach (var worm in _worms)
            {
                stringBuilder.Append($"{worm}, ");
            }

            if (_worms.Count != 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }

            stringBuilder.Append(']');

            stringBuilder.Append(", Food: [");

            foreach (var food in _food)
            {
                stringBuilder.Append($"{food.Position}, ");
            }

            if (_food.Count != 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }

            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }

        public string MapToString(int leftBorder = -5, int rightBorder = 5, int topBorder = 5, int bottomBorder = -5)
        {
            throw new NotImplementedException(); //TODO
        }

        protected bool Exists(Worm worm)
        {
            return _worms.Contains(worm);
        }
    }
}