using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NSU.Worm
{
    public class Simulator : ISimulator
    {
        private readonly IWormBehaviourProvider _wormBehaviourProvider;
        private readonly IFoodGenerator _foodGenerator;
        private readonly INameGenerator _nameGenerator;

        private readonly SimulatorOptions _options;
        private readonly ILogger<ISimulator> _logger;

        private readonly IMutableWorldState _worldState;
        private long _iteration;

        public Simulator(IWormBehaviourProvider wormBehaviourProvider, IFoodGenerator foodGenerator,
            INameGenerator nameGenerator, IMutableWorldState worldState, ILogger<ISimulator> logger,
            IOptions<SimulatorOptions> options)
        {
            _wormBehaviourProvider = wormBehaviourProvider;
            _foodGenerator = foodGenerator;
            _nameGenerator = nameGenerator;
            _logger = logger;
            _worldState = worldState;

            _options = options.Value;

            if (_options.NamePool.Count != 0)
            {
                _nameGenerator.NamePool = new List<string>(_options.NamePool);
            }

            InitDefaultWorms();

            _iteration = 0;
        }

        public void Start()
        {
            Start(_options.Iterations);
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

                if (food.Freshness <= _options.FoodFreshnessBorderline)
                {
                    _worldState.Remove(food);
                }
            }

            var newFood = _foodGenerator.GenerateFood(_options.FoodFreshnessStart);

            while (_worldState.Get(newFood.Position) == Tile.Food)
            {
                newFood = _foodGenerator.GenerateFood(_options.FoodFreshnessStart);
            }

            if (_worldState.Get(newFood.Position) == Tile.Worm)
            {
                _worldState.GetWorm(newFood.Position).Life += _options.FoodLifeGain;
            }
            else
            {
                _worldState.Put(newFood);
            }

            foreach (var worm in _worldState.Worms)
            {
                worm.Life--;

                if (worm.Life <= _options.WormLifeBorderline)
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
                worm.Life += _options.FoodLifeGain;
            }

            if (_worldState.Get(newPosition) != Tile.Worm)
            {
                _worldState.Move(worm, newPosition);
            }
        }

        private void ReproduceWorm(Worm worm, Direction direction)
        {
            var childPosition = worm.Position.Next(direction);

            if (worm.Life <= _options.ReproductionLifeRequirement || _worldState.Get(childPosition) != Tile.Empty)
            {
                return;
            }

            worm.Life -= _options.ReproductionLifeCost;

            var childWorm = new Worm(_nameGenerator.NextName(), _options.WormLifeStart, childPosition);
            var childBehaviour = _wormBehaviourProvider.GetBehaviour(worm).CopyForWorm(childWorm);

            AddWorm(childWorm, childBehaviour);
        }

        private void AddWorm(Worm worm, IWormBehaviour behaviour)
        {
            _wormBehaviourProvider.RegisterBehaviour(worm, behaviour);
            _worldState.Put(worm);
        }

        private void LogState()
        {
            StringBuilder stringBuilder = new();

            if (_iteration == 0)
            {
                stringBuilder.AppendLine("Worm Simulator started!");
            }

            stringBuilder.Append($"Iteration: {_iteration}\t");
            stringBuilder.Append(_worldState.StateToString());

            _logger.LogInformation(stringBuilder.ToString());
        }

        private void InitDefaultWorms()
        {
            var defaultWorms = _options.StartWorms;

            foreach (var worm in defaultWorms)
            {
                // TODO: возможность конфигурации поведения
                var behaviour = new CirclingWormBehaviour(worm, worm.Position);
                AddWorm(worm, behaviour);
            }
        }
    }
}