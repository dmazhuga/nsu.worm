using System;
using System.Collections.Generic;
using System.Text;

namespace NSU.Worm
{
    public class Simulator
    {
        private readonly WorldState _worldState;

        private long _iteration;

        public Simulator(List<Worm> worms)
        {
            _worldState = new ArrayCacheWorldState(worms);

            _iteration = 0;
        }

        public void Start(int iterations)
        {
            PrintState();

            while (_iteration < iterations)
            {
                Iteration();
                PrintState();
            }
        }

        private void Iteration()
        {
            foreach (var worm in _worldState.Worms)
            {
                var action = worm.GetAction();

                switch (action.Type)
                {
                    case WormAction.ActionType.Move:
                        MoveWorm(worm, action.Direction);
                        break;
                    case WormAction.ActionType.DoNothing:
                        break;
                    default:
                        throw new ArgumentException($"Unsupported action type: {action.Type}");
                }
            }

            _iteration++;
        }

        private void MoveWorm(Worm worm, Direction direction)
        {
            var newPosition = worm.Position.Next(direction);

            if (_worldState.Get(newPosition) != WorldState.Tile.Worm)
            {
                _worldState.Move(worm, newPosition);
            }
        }

        private void PrintState()
        {
            var stringBuilder = new StringBuilder();

            if (_iteration == 0)
            {
                stringBuilder.AppendLine("Worm Simulator started!");
            }

            stringBuilder.Append($"Iteration: {_iteration}\t");
            stringBuilder.AppendLine(_worldState.StateToString());

            Console.Write(stringBuilder.ToString());
        }
    }
}