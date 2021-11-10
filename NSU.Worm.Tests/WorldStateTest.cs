using System.Collections.Generic;
using NUnit.Framework;

namespace NSU.Worm.Tests
{
    [TestFixture]
    public class WorldStateTest
    {
        private const string WormName = "Name";
        private const int WormLife = 10;
        private const int WormPositionX = 0;
        private const int WormPositionY = 0;

        private Worm Worm;

        private const string OtherWormName = "OtherName";
        private const int OtherWormLife = 10;
        private const int OtherWormPositionX = 1;
        private const int OtherWormPositionY = 1;

        private Worm OtherWorm;

        private const int FoodFreshness = 10;
        private const int FoodPositionX = 2;
        private const int FoodPositionY = 2;

        private Food Food;

        private const int OtherFoodFreshness = 10;
        private const int OtherFoodPositionX = 3;
        private const int OtherFoodPositionY = 3;

        private Food OtherFood;

        private const int EmptyPositionX = 4;
        private const int EmptyPositionY = 4;

        private WorldState _worldState;

        [SetUp]
        public void SetUp()
        {
            Worm = new Worm(WormName, WormLife, WormPositionX, WormPositionY);
            OtherWorm = new Worm(OtherWormName, OtherWormLife, OtherWormPositionX, OtherWormPositionY);
            Food = new Food(FoodPositionX, FoodPositionY, FoodFreshness);
            OtherFood = new Food(OtherFoodPositionX, OtherFoodPositionY, OtherFoodFreshness);
        }

        [TestCase(WormName, true)]
        [TestCase(WormName + "A", false)]
        public void TestGetWormByName(string name, bool shouldSucceed)
        {
            _worldState = new WorldState(new List<Worm> {Worm});
            var expected = shouldSucceed ? Worm : null;

            var result = _worldState.GetWorm(name);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(WormPositionX, WormPositionY, true)]
        [TestCase(FoodPositionX, FoodPositionY, false)]
        [TestCase(EmptyPositionX, EmptyPositionY, false)]
        public void TestGetWormByPosition(int positionX, int positionY, bool shouldSucceed)
        {
            _worldState = new WorldState(new List<Worm> {Worm}, new List<Food> {Food});
            var expected = shouldSucceed ? Worm : null;

            var result = _worldState.GetWorm(new Position(positionX, positionY));

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(FoodPositionX, FoodPositionY, true)]
        [TestCase(WormPositionX, WormPositionY, false)]
        [TestCase(EmptyPositionX, EmptyPositionY, false)]
        public void TestGetFoodByPosition(int positionX, int positionY, bool shouldSucceed)
        {
            _worldState = new WorldState(new List<Worm> {Worm}, new List<Food> {Food});
            var expected = shouldSucceed ? Food : null;

            var result = _worldState.GetFood(new Position(positionX, positionY));

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TestGet()
        {
            _worldState = new WorldState(new List<Worm> {Worm}, new List<Food> {Food});

            var resultTileWormPosition = _worldState.Get(new Position(WormPositionX, WormPositionY));
            var resultTileFoodPosition = _worldState.Get(new Position(FoodPositionX, FoodPositionY));
            var resultTileEmptyPosition = _worldState.Get(new Position(EmptyPositionX, EmptyPositionY));

            Assert.That(resultTileWormPosition, Is.EqualTo(Tile.Worm));
            Assert.That(resultTileFoodPosition, Is.EqualTo(Tile.Food));
            Assert.That(resultTileEmptyPosition, Is.EqualTo(Tile.Empty));
        }

        [Test]
        public void TestReadonlyLists()
        {
            var worms = new List<Worm> {Worm};
            var food = new List<Food> {Food};
            _worldState = new WorldState(worms, food);

            var resultWorms = _worldState.Worms;
            var resultFood = _worldState.Food;

            Assert.That(resultWorms, Is.EquivalentTo(worms));
            Assert.That(resultFood, Is.EquivalentTo(food));
        }

        /// <summary>
        /// Тестирует возможность передвинуть червя. Важно: состояние мира не реализует игровых правил,
        /// но вместо этого требует, чтобы на одной клетке находился только один объект. Поэтому
        /// при передвижении червя на клетку с едой должна быть выброшена ошибка.
        /// </summary>
        [TestCase(EmptyPositionX, EmptyPositionY, true)]
        [TestCase(WormPositionX, WormPositionY, true)]
        [TestCase(OtherWormPositionX, OtherWormPositionY, false)]
        [TestCase(FoodPositionX, FoodPositionY, false)]
        public void TestMoveWorm(int positionToX, int positionToY, bool shouldSucceed)
        {
            _worldState = new WorldState(new List<Worm> {Worm, OtherWorm}, new List<Food> {Food});
            var newPosition = new Position(positionToX, positionToY);

            if (shouldSucceed)
            {
                _worldState.Move(Worm, newPosition);

                var resultGetWormByPosition = _worldState.GetWorm(newPosition);
                var resultGetWormByName = _worldState.GetWorm(Worm.Name);

                Assert.That(resultGetWormByPosition, Is.EqualTo(Worm));
                Assert.That(resultGetWormByName, Is.EqualTo(Worm));

                Assert.That(Worm.Position, Is.EqualTo(newPosition));
                Assert.That(Food.Position, Is.EqualTo(new Position(FoodPositionX, FoodPositionY)));
                Assert.That(OtherWorm.Position, Is.EqualTo(new Position(OtherWormPositionX, OtherWormPositionY)));
            }
            else
            {
                Assert.Catch(() => _worldState.Move(Worm, newPosition));
                Assert.That(_worldState.GetWorm(newPosition), Is.Not.EqualTo(Worm));
            }
        }

        [TestCase(EmptyPositionX, EmptyPositionY, true)]
        [TestCase(WormPositionX, WormPositionY, true)]
        [TestCase(OtherWormPositionX, OtherWormPositionY, false)]
        [TestCase(FoodPositionX, FoodPositionY, false)]
        public void TestPutWorm(int positionToX, int positionToY, bool shouldSucceed)
        {
            _worldState = new WorldState(new List<Worm> {OtherWorm}, new List<Food> {Food});
            var newPosition = new Position(positionToX, positionToY);

            if (shouldSucceed)
            {
                _worldState.Put(Worm, newPosition);

                var resultGetWormByPosition = _worldState.GetWorm(newPosition);
                var resultGetWormByName = _worldState.GetWorm(Worm.Name);

                Assert.That(resultGetWormByPosition, Is.EqualTo(Worm));
                Assert.That(resultGetWormByName, Is.EqualTo(Worm));

                Assert.That(Worm.Position, Is.EqualTo(newPosition));
                Assert.That(Food.Position, Is.EqualTo(new Position(FoodPositionX, FoodPositionY)));
                Assert.That(OtherWorm.Position, Is.EqualTo(new Position(OtherWormPositionX, OtherWormPositionY)));
            }
            else
            {
                Assert.Catch(() => _worldState.Put(Worm, newPosition));
                Assert.That(_worldState.GetWorm(newPosition), Is.Not.EqualTo(Worm));
            }
        }

        [TestCase(EmptyPositionX, EmptyPositionY, true)]
        [TestCase(WormPositionX, WormPositionY, false)]
        [TestCase(OtherFoodPositionX, OtherFoodPositionY, false)]
        public void TestPutFood(int positionToX, int positionToY, bool shouldSucceed)
        {
            _worldState = new WorldState(new List<Worm> {Worm}, new List<Food> {OtherFood});
            var newPosition = new Position(positionToX, positionToY);

            if (shouldSucceed)
            {
                _worldState.Put(Food, newPosition);

                var resultGetFood = _worldState.GetFood(newPosition);

                Assert.That(resultGetFood, Is.EqualTo(Food));

                Assert.That(Food.Position, Is.EqualTo(newPosition));
                Assert.That(Worm.Position, Is.EqualTo(new Position(WormPositionX, WormPositionY)));
                Assert.That(OtherFood.Position, Is.EqualTo(new Position(OtherFoodPositionX, OtherFoodPositionY)));
            }
            else
            {
                Assert.Catch(() => _worldState.Put(Food, newPosition));
                Assert.That(_worldState.GetFood(newPosition), Is.Not.EqualTo(Food));
            }
        }

        [Test]
        public void TestRemoveWorm()
        {
            _worldState = new WorldState(new List<Worm> {Worm, OtherWorm}, new List<Food> {Food});
            
            _worldState.Remove(Worm);
            
            Assert.That(_worldState.Worms.Contains(Worm), Is.False);
            Assert.That(_worldState.Worms.Contains(OtherWorm), Is.True);
            Assert.That(_worldState.Food.Contains(Food), Is.True);
            
            Assert.Catch(() => _worldState.Remove(Worm));
        }
        
        [Test]
        public void TestRemoveFood()
        {
            _worldState = new WorldState(new List<Worm> {Worm}, new List<Food> {Food, OtherFood});
            
            _worldState.Remove(Food);
            
            Assert.That(_worldState.Food.Contains(Food), Is.False);
            Assert.That(_worldState.Food.Contains(OtherFood), Is.True);
            Assert.That(_worldState.Worms.Contains(Worm), Is.True);
            
            Assert.Catch(() => _worldState.Remove(Food));
        }
    }
}