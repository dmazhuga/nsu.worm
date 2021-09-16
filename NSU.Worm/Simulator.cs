using System;
using System.Collections.Generic;
using System.Text;

namespace NSU.Worm
{
    public class Simulator
    {
        private readonly WorldState _worldState;

        private readonly FoodGenerator _foodGenerator;

        private long _iteration;

        public Simulator(List<Worm> worms)
        {
            _worldState = new BaseWorldState(worms);
            _foodGenerator = new FoodGenerator();

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
            foreach (var food in _worldState.Food)
            {
                food.Freshness--;

                if (food.Freshness <= 0)
                {
                    _worldState.Remove(food);
                }
            }

            var newFood = _foodGenerator.GenerateFood();

            while (_worldState.Get(newFood.Position) == WorldState.Tile.Food)
            {
                newFood = _foodGenerator.GenerateFood();
            }

            if (_worldState.Get(newFood.Position) == WorldState.Tile.Worm)
            {
                 _worldState.GetWorm(newFood.Position).Life += 10;
            }
            else
            {
                _worldState.Put(newFood, newFood.Position);
            }

            foreach (var worm in _worldState.Worms)
            {
                worm.Life--;

                if (worm.Life <= 0)
                {
                    _worldState.Remove(worm);
                    continue;
                }

                var action = worm.GetAction();

                switch (action.Type)
                {
                    case WormAction.ActionType.Move:
                        MoveWorm(worm, action.Direction);
                        break;
                    case WormAction.ActionType.Reproduce:
                        ReproduceWorm(worm, action.Direction);
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
            
            if (_worldState.Get(newPosition) == WorldState.Tile.Food)
            {
                _worldState.Remove(_worldState.GetFood(newPosition));
                worm.Life += 10;
            }

            if (_worldState.Get(newPosition) != WorldState.Tile.Worm)
            {
                _worldState.Move(worm, newPosition);
            }
        }

        private void ReproduceWorm(Worm worm, Direction direction)
        {
            var childPosition = worm.Position.Next(direction);

            if (worm.Life <= 10 || _worldState.Get(childPosition) != WorldState.Tile.Empty)
            {
                return;
            }

            worm.Life -= 10;
            var childWorm = worm.Reproduce(worm.Name + "Jr", childPosition, 10);     //TODO: name generation
            
            _worldState.Put(childWorm, childPosition);
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