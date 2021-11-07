using System;
using System.Collections.Generic;
using System.Text;

namespace NSU.Worm
{
    public class Simulator
    {
        private readonly IMutableWorldState _worldState;

        private readonly WormBehaviourProvider _wormBehaviourProvider;

        private readonly FoodGenerator _foodGenerator;

        private readonly NameGenerator _nameGenerator;

        private readonly Logger _logger;

        private long _iteration;

        public Simulator(List<KeyValuePair<Worm, IWormBehaviour>> worms)
        {
            _worldState = new WorldState();
            _wormBehaviourProvider = new WormBehaviourProvider();
            _foodGenerator = new FoodGenerator();
            _nameGenerator = new NameGenerator();
            _logger = new Logger(true, true);

            foreach (var (worm, behaviour) in worms)
            {
                AddWorm(worm, behaviour);
            }
            
            _iteration = 0;
        }

        public void Start(int iterations)
        {
            LogState();

            while (_iteration < iterations)
            {
                Iteration();
                LogState();
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

            while (_worldState.Get(newFood.Position) == Tile.Food)
            {
                newFood = _foodGenerator.GenerateFood();
            }

            if (_worldState.Get(newFood.Position) == Tile.Worm)
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

                var behaviour = _wormBehaviourProvider.GetBehaviour(worm);
                var action = behaviour.GetAction(_worldState);

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

            if (_worldState.Get(newPosition) == Tile.Food)
            {
                _worldState.Remove(_worldState.GetFood(newPosition));
                worm.Life += 10;
            }

            if (_worldState.Get(newPosition) != Tile.Worm)
            {
                _worldState.Move(worm, newPosition);
            }
        }

        private void ReproduceWorm(Worm worm, Direction direction)
        {
            var childPosition = worm.Position.Next(direction);

            if (worm.Life <= 10 || _worldState.Get(childPosition) != Tile.Empty)
            {
                return;
            }

            worm.Life -= 10;
            
            var childWorm = new Worm(_nameGenerator.NextName(), 10, childPosition);
            var childBehaviour = _wormBehaviourProvider.GetBehaviour(worm).CopyForWorm(childWorm);

            AddWorm(childWorm, childBehaviour);
        }
        
        private void AddWorm(Worm worm, IWormBehaviour behaviour)
        {
            _wormBehaviourProvider.RegisterBehaviour(worm, behaviour);
            _worldState.Put(worm, worm.Position);
        }

        private void LogState()
        {
            StringBuilder stringBuilder = new();

            if (_iteration == 0)
            {
                stringBuilder.AppendLine("Worm Simulator started!");
            }

            stringBuilder.Append($"Iteration: {_iteration}\t");
            stringBuilder.AppendLine(_worldState.StateToString());

            _logger.log(stringBuilder.ToString());
        }
    }
}