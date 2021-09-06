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

        public BaseWorldState(List<Worm> worms)
        {
            _worms = new List<Worm>();

            foreach (var worm in worms)
            {
                Put(worm, worm.Position);
            }
        }

        public ImmutableList<Worm> Worms => _worms.ToImmutableList();

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
                    $"Cannot move worm to position {position} - it is used by other worm");
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

        public virtual WorldState.Tile Get(Position position)
        {
            return _worms.Any(worm => worm.Position == position) ? WorldState.Tile.Worm : WorldState.Tile.Empty;
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
            
            return stringBuilder.ToString();
        }

        public string MapToString(int leftBorder = -5, int rightBorder = 5, int topBorder = 5, int bottomBorder = -5)
        {
            throw new NotImplementedException();    //TODO
        }

        protected bool Exists(Worm worm)
        {
            return _worms.Contains(worm);
        }
    }
}