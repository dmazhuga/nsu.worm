using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSU.Worm
{
    public class Simulator
    {
        private List<Worm> _worms;

        private long _iteration;

        public Simulator(List<Worm> worms)
        {
            _worms = worms;

            _iteration = 0;
        }

        public void Start()
        {
            PrintState();

            while (_iteration < 10)
            {
                Iteration();
                PrintState();
            }
        }

        private void Iteration()
        {
            foreach (var worm in _worms)
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
            Position newPosition;
            
            switch (direction)
            {
                case Direction.Left:
                    newPosition = worm.Position.NextLeft();
                    break;
                case Direction.Right:
                    newPosition = worm.Position.NextRight();
                    break;
                case Direction.Down:
                    newPosition = worm.Position.NextDown();
                    break;
                case Direction.Up:
                    newPosition = worm.Position.NextUp();
                    break; 
                default:
                    throw new ArgumentException($"Unsupported direction argument: {direction}");
            }
            
            MoveWorm(worm, newPosition);
        }

        private void MoveWorm(Worm worm, Position newPosition)
        {
            if (_worms.Any(otherWorm => otherWorm != worm && otherWorm.Position == newPosition))
            {
                return;
            }

            worm.Position = newPosition;
        }

        private void PrintState()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (_iteration == 0)
            {
                stringBuilder.Append("Worm Simulator started!\n");
            }

            stringBuilder.Append($"Iteration: {_iteration}\tWorms: [");

            foreach (var worm in _worms)
            {
                stringBuilder.Append($"{worm}, ");
            }

            if (_worms.Count != 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            
            stringBuilder.Append(']');

            Console.WriteLine(stringBuilder.ToString());
        }
    }
}