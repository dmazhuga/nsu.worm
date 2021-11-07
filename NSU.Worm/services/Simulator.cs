using System;
using System.Text;

namespace NSU.Worm
{
    public class Simulator : ISimulator
    {
        private const int DefaultIterations = 100;

        private readonly IWormBehaviourProvider _wormBehaviourProvider;
        private readonly IFoodGenerator _foodGenerator;
        private readonly INameGenerator _nameGenerator;

        private readonly Logger _logger;

        private readonly IMutableWorldState _worldState;
        private long _iteration;

        public Simulator(IWormBehaviourProvider wormBehaviourProvider, IFoodGenerator foodGenerator,
            INameGenerator nameGenerator)
        {
            _wormBehaviourProvider = wormBehaviourProvider;
            _foodGenerator = foodGenerator;
            _nameGenerator = nameGenerator;

            _worldState = new WorldState();
            _logger = new Logger(true, true);

            InitDefaultWorms();

            _iteration = 0;
        }

        public void Start()
        {
            Start(DefaultIterations);
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

        private void InitDefaultWorms()
        {
            var worm1 = new Worm("Sasha", 10, 2, 0);
            var behaviour1 = new CirclingWormBehaviour(worm1, worm1.Position);

            var worm2 = new Worm("Zhenya", 10, -2, 0);
            var behaviour2 = new CirclingWormBehaviour(worm2, worm2.Position);

            AddWorm(worm1, behaviour1);
            AddWorm(worm2, behaviour2);
        }
    }
}