using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace NSU.Worm.Tests
{
    [TestFixture]
    public class SimulatorTest
    {
        private Simulator _simulator;

        private IWormBehaviourProvider _wormBehaviourProvider;
        private IFoodGenerator _foodGenerator;
        private INameGenerator _nameGenerator;
        private IMutableWorldState _worldState;
        private IOptions<SimulatorOptions> _options;
        private ILogger<ISimulator> _logger;

        private const string WormName = "Name";
        private const int WormLife = 10;
        private const int WormPositionX = 0;
        private const int WormPositionY = 0;

        [SetUp]
        public void SetUp()
        {
            _wormBehaviourProvider = Substitute.For<IWormBehaviourProvider>();

            _foodGenerator = Substitute.For<IFoodGenerator>();
            _foodGenerator.GenerateFood(default).ReturnsForAnyArgs(new Food(0, 0, 10));

            _nameGenerator = Substitute.For<INameGenerator>();
            _nameGenerator.NextName().Returns("NextName");

            _worldState = Substitute.For<IMutableWorldState>();
            _worldState.Food.Returns(ImmutableList<Food>.Empty);
            _worldState.Worms.Returns(ImmutableList<Worm>.Empty);

            _logger = Substitute.For<ILogger<ISimulator>>();

            _options = Substitute.For<IOptions<SimulatorOptions>>();
            _options.Value.Returns(new SimulatorOptions
            {
                Iterations = 1,
                FoodLifeGain = 10
            });
            
            _simulator = new Simulator(_wormBehaviourProvider, _foodGenerator, _nameGenerator, _worldState,
                _logger, _options);
        }
        
        [Test]
        public void TestGenerateAndPutFood()
        {
            var food = new Food(0, 0, 10);
            _foodGenerator.GenerateFood(default).ReturnsForAnyArgs(food);

            _simulator.Start(1);
            
            _worldState.Received().Put(food);
        }
        
        [Test]
        public void TestGenerateFoodAgain()
        {
            var position = new Position(WormPositionX, WormPositionY);
            _worldState.Get(position).Returns(Tile.Food);
            
            var foodBadPosition = new Food(position, 10);
            var foodGoodPosition = new Food(position.Next(Direction.Down), 10);
            _foodGenerator.GenerateFood(default).ReturnsForAnyArgs(foodBadPosition, foodGoodPosition);

            _simulator.Start(1);

            _foodGenerator.Received(2).GenerateFood(Arg.Any<int>());
            _worldState.Received().Put(foodGoodPosition);
        }
        
        [Test]
        public void TestGenerateFoodOnWorm()
        {
            var position = new Position(WormPositionX, WormPositionY);
            var worm = new Worm(WormName, WormLife, position);
            _worldState.GetWorm(position).Returns(worm);
            _worldState.Get(position).Returns(Tile.Worm);
            
            var food = new Food(position, 10);
            _foodGenerator.GenerateFood(default).ReturnsForAnyArgs(food);

            _simulator.Start(1);

            _foodGenerator.Received().GenerateFood(Arg.Any<int>());
            _worldState.DidNotReceiveWithAnyArgs().Put(food);
            Assert.That(worm.Life, Is.EqualTo(WormLife + 10));
        }
        
        [Test]
        public void TestFoodLoseFreshness()
        {
            var foodFreshness = 10;
            var food = new Food(1, 1, foodFreshness);
            var foodList = ImmutableList.Create(food);
            _worldState.Food.Returns(foodList);

            _simulator.Start(1);
            
            Assert.That(food.Freshness, Is.EqualTo(foodFreshness - 1));
            _worldState.DidNotReceiveWithAnyArgs().Remove(Arg.Any<Food>());
        }
        
        [Test]
        public void TestRemoveFoodWithNoFreshness()
        {
            var foodFreshness = 1;
            var food = new Food(1, 1, foodFreshness);
            var foodList = ImmutableList.Create(food);
            _worldState.Food.Returns(foodList);

            _simulator.Start(1);
            
            // снижается на единицу в начале итерации, затем удаляется
            _worldState.Received().Remove(food);
        }
        
        [Test]
        public void TestRemoveWormWithNoLife()
        {
            var wormLife = 1;
            var position = new Position(WormPositionX, WormPositionY);
            var worm = new Worm(WormName, wormLife, position);
            var worms = ImmutableList.Create(worm);
            _worldState.Worms.Returns(worms);

            _simulator.Start(1);
            
            // снижается на единицу в начале итерации, затем удаляется
            _worldState.Received().Remove(worm);
        }
        
        [Test]
        public void TestWormDoNothing()
        {
            var position = new Position(WormPositionX, WormPositionY);
            var worm = new Worm(WormName, WormLife, position);
            var worms = ImmutableList.Create(worm);
            _worldState.Worms.Returns(worms);

            var behaviour = Substitute.For<IWormBehaviour>();
            behaviour.GetAction(default).ReturnsForAnyArgs(WormAction.DoNothing);
            _wormBehaviourProvider.GetBehaviour(worm).Returns(behaviour);

            _simulator.Start(1);
            
            _worldState.DidNotReceiveWithAnyArgs().Move(default, default);
            _worldState.DidNotReceiveWithAnyArgs().Put(Arg.Any<Worm>());
            Assert.That(worm.Position, Is.EqualTo(position));
            Assert.That(worm.Life, Is.EqualTo(WormLife - 1));
        }

        [Test]
        public void TestMoveWormToEmptyTile()
        {
            var position = new Position(WormPositionX, WormPositionY);
            var worm = new Worm(WormName, WormLife, position);
            var worms = ImmutableList.Create(worm);
            _worldState.Worms.Returns(worms);

            var behaviour = Substitute.For<IWormBehaviour>();
            behaviour.GetAction(default).ReturnsForAnyArgs(WormAction.MoveUp);
            _wormBehaviourProvider.GetBehaviour(worm).Returns(behaviour);

            _simulator.Start(1);
            
            _worldState.Received().Move(worm, position.Next(Direction.Up));
            Assert.That(worm.Life, Is.EqualTo(WormLife - 1));
        }
        
        [Test]
        public void TestMoveWormToTileWithFood()
        {
            var wormPosition = new Position(WormPositionX, WormPositionY);
            var worm = new Worm(WormName, WormLife, wormPosition);
            var worms = ImmutableList.Create(worm);
            _worldState.Worms.Returns(worms);

            var behaviour = Substitute.For<IWormBehaviour>();
            behaviour.GetAction(default).ReturnsForAnyArgs(WormAction.MoveUp);
            _wormBehaviourProvider.GetBehaviour(worm).Returns(behaviour);
            
            var foodPosition = wormPosition.Next(Direction.Up);
            var food = new Food(foodPosition, 10);
            _worldState.GetFood(foodPosition).Returns(food);
            _worldState.Get(foodPosition).Returns(Tile.Food);

            _simulator.Start(1);
            
            _worldState.Received().Move(worm, wormPosition.Next(Direction.Up));
            _worldState.Received().Remove(food);
            Assert.That(worm.Life, Is.EqualTo(WormLife - 1 + 10));
        }
        
        [Test]
        public void TestMoveWormToTileWithOtherWorm()
        {
            var position = new Position(WormPositionX, WormPositionY);
            var worm = new Worm(WormName, WormLife, position);
            var worms = ImmutableList.Create(worm);
            _worldState.Worms.Returns(worms);
            
            var otherWormPosition = position.Next(Direction.Up);
            _worldState.Get(otherWormPosition).Returns(Tile.Worm);

            var behaviour = Substitute.For<IWormBehaviour>();
            behaviour.GetAction(default).ReturnsForAnyArgs(WormAction.MoveUp);
            _wormBehaviourProvider.GetBehaviour(worm).Returns(behaviour);

            _simulator.Start(1);
            
            _worldState.DidNotReceiveWithAnyArgs().Move(default, default);
            _worldState.DidNotReceiveWithAnyArgs().Remove(Arg.Any<Worm>());
            _worldState.DidNotReceiveWithAnyArgs().Put(Arg.Any<Worm>());
            Assert.That(worm.Position, Is.EqualTo(position));
            Assert.That(worm.Life, Is.EqualTo(WormLife - 1));
        }
        
        [Test]
        public void TestReproductionUnsuccessfulTileNotEmpty()
        {
            var bigWormLife = WormLife + 10;
            var position = new Position(WormPositionX, WormPositionY);
            var worm = new Worm(WormName, bigWormLife, position);
            var worms = ImmutableList.Create(worm);
            _worldState.Worms.Returns(worms);
            
            var reproducePosition = position.Next(Direction.Up);
            _worldState.Get(reproducePosition).Returns(Tile.Worm);

            var behaviour = Substitute.For<IWormBehaviour>();
            behaviour.GetAction(default).ReturnsForAnyArgs(WormAction.ReproduceUp);
            _wormBehaviourProvider.GetBehaviour(worm).Returns(behaviour);

            _simulator.Start(1);
            
            _worldState.DidNotReceiveWithAnyArgs().Move(default, default);
            _worldState.DidNotReceiveWithAnyArgs().Remove(Arg.Any<Worm>());
            _worldState.DidNotReceiveWithAnyArgs().Put(Arg.Any<Worm>());
            Assert.That(worm.Position, Is.EqualTo(position));
            Assert.That(worm.Life, Is.EqualTo(bigWormLife - 1));
        }
        
        [Test]
        public void TestReproductionUnsuccessfulLifeNotEnough()
        {
            var position = new Position(WormPositionX, WormPositionY);
            var worm = new Worm(WormName, WormLife, position);
            var worms = ImmutableList.Create(worm);
            _worldState.Worms.Returns(worms);
            
            var reproducePosition = position.Next(Direction.Up);
            _worldState.Get(reproducePosition).Returns(Tile.Empty);

            var behaviour = Substitute.For<IWormBehaviour>();
            behaviour.GetAction(default).ReturnsForAnyArgs(WormAction.ReproduceUp);
            _wormBehaviourProvider.GetBehaviour(worm).Returns(behaviour);

            _simulator.Start(1);
            
            _worldState.DidNotReceiveWithAnyArgs().Move(default, default);
            _worldState.DidNotReceiveWithAnyArgs().Remove(Arg.Any<Worm>());
            _worldState.DidNotReceiveWithAnyArgs().Put(Arg.Any<Worm>());
            Assert.That(worm.Position, Is.EqualTo(position));
            Assert.That(worm.Life, Is.EqualTo(WormLife - 1));
        }
        
        [Test]
        public void TestReproductionSuccessful()
        {
            var bigWormLife = WormLife + 10;
            var position = new Position(WormPositionX, WormPositionY);
            var worm = new Worm(WormName, bigWormLife, position);
            var worms = ImmutableList.Create(worm);
            _worldState.Worms.Returns(worms);
            
            var reproducePosition = position.Next(Direction.Up);
            _worldState.Get(reproducePosition).Returns(Tile.Empty);

            var behaviour = Substitute.For<IWormBehaviour>();
            behaviour.GetAction(default).ReturnsForAnyArgs(WormAction.ReproduceUp);
            behaviour.CopyForWorm(default).ReturnsForAnyArgs(behaviour);
            _wormBehaviourProvider.GetBehaviour(worm).Returns(behaviour);

            var childName = "ChildName";
            _nameGenerator.NextName().Returns(childName);

            _simulator.Start(1);
            
            _worldState.DidNotReceiveWithAnyArgs().Move(default, default);
            _worldState.DidNotReceiveWithAnyArgs().Remove(Arg.Any<Worm>());
            _worldState.Received().Put(Arg.Any<Worm>());
            behaviour.ReceivedWithAnyArgs(1).CopyForWorm(default);
            _nameGenerator.Received().NextName();
            _wormBehaviourProvider.Received().RegisterBehaviour(Arg.Any<Worm>(), behaviour);
            Assert.That(worm.Life, Is.EqualTo(bigWormLife - 1 - 10));
        }
    }
}