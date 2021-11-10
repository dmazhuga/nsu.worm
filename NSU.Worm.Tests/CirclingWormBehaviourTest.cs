using NSubstitute;
using NUnit.Framework;

namespace NSU.Worm.Tests
{
    [TestFixture]
    public class CirclingWormBehaviourTest
    {
        private const string Name = "Name";
        private const int LowLife = 10;
        private const int ReproducingLife = 15;
        private static readonly Position Position = new(0, 0);

        private Worm _worm;
        private IWorldState _state;

        private CirclingWormBehaviour _circlingWormBehaviour;

        [SetUp]
        public void SetUp()
        {
            _worm = new Worm(Name, 10, Position);
            
            _state = Substitute.For<IWorldState>();
            _state.GetWorm(Name).Returns(_worm);

            _circlingWormBehaviour = new CirclingWormBehaviour(_worm, Position);
        }
        
        /// <summary>
        /// Червь в центре круга, должен пойти наверх.
        /// </summary>
        [Test]
        public void TestGetActionWormInCenter()
        {
            var result = _circlingWormBehaviour.GetAction(_state);
            Assert.That(result, Is.EqualTo(WormAction.MoveUp));
        }
        
        /// <summary>
        /// Червь над центром, должен пойти вправо.
        /// </summary>
        [Test]
        public void TestGetActionWormUp()
        {
            _worm.Position = Position.Next(Direction.Up);
            
            var result = _circlingWormBehaviour.GetAction(_state);
            Assert.That(result, Is.EqualTo(WormAction.MoveRight));
        }
        
        /// <summary>
        /// Червь справа сверху над центром, должен пойти вниз.
        /// </summary>
        [Test]
        public void TestGetActionWormUpRight()
        {
            _worm.Position = Position.Next(Direction.UpRight);
            
            var result = _circlingWormBehaviour.GetAction(_state);
            Assert.That(result, Is.EqualTo(WormAction.MoveDown));
        }
        
        /// <summary>
        /// Червь справа от центра, должен пойти вниз.
        /// </summary>
        [Test]
        public void TestGetActionWormRight()
        {
            _worm.Position = Position.Next(Direction.Right);
            
            var result = _circlingWormBehaviour.GetAction(_state);
            Assert.That(result, Is.EqualTo(WormAction.MoveDown));
        }
        
        /// <summary>
        /// Червь справа снизу от центра, должен пойти влево.
        /// </summary>
        [Test]
        public void TestGetActionWormDownRight()
        {
            _worm.Position = Position.Next(Direction.DownRight);
            
            var result = _circlingWormBehaviour.GetAction(_state);
            Assert.That(result, Is.EqualTo(WormAction.MoveLeft));
        }
        
        /// <summary>
        /// Червь снизу от центра, должен пойти влево.
        /// </summary>
        [Test]
        public void TestGetActionWormDown()
        {
            _worm.Position = Position.Next(Direction.Down);
            
            var result = _circlingWormBehaviour.GetAction(_state);
            Assert.That(result, Is.EqualTo(WormAction.MoveLeft));
        }
        
        /// <summary>
        /// Червь снизу слева от центра, должен пойти вверх.
        /// </summary>
        [Test]
        public void TestGetActionWormDownLeft()
        {
            _worm.Position = Position.Next(Direction.DownLeft);
            
            var result = _circlingWormBehaviour.GetAction(_state);
            Assert.That(result, Is.EqualTo(WormAction.MoveUp));
        }
        
        /// <summary>
        /// Червь слева от центра, должен пойти вверх.
        /// </summary>
        [Test]
        public void TestGetActionWormLeft()
        {
            _worm.Position = Position.Next(Direction.Left);
            
            var result = _circlingWormBehaviour.GetAction(_state);
            Assert.That(result, Is.EqualTo(WormAction.MoveUp));
        }
        
        /// <summary>
        /// Червь слева сверху над центром, должен пойти вправо.
        /// </summary>
        [Test]
        public void TestGetActionWormUpLeft()
        {
            _worm.Position = Position.Next(Direction.UpLeft);
            
            var result = _circlingWormBehaviour.GetAction(_state);
            Assert.That(result, Is.EqualTo(WormAction.MoveRight));
        }
        
        /// <summary>
        /// Червь вне круга, ничего не делает.
        /// </summary>
        [Test]
        public void TestGetActionWormOutsideCircle()
        {
            _worm.Position = new Position(10, 10);
            
            var result = _circlingWormBehaviour.GetAction(_state);
            Assert.That(result, Is.EqualTo(WormAction.DoNothing));
        }
        
        /// <summary>
        /// При высоком уровне жизни червь должен размножиться.
        /// </summary>
        [TestCase(LowLife, WormAction.ActionType.DoNothing)]
        [TestCase(ReproducingLife, WormAction.ActionType.Reproduce)]
        public void TestGetActionReproduction(int life, WormAction.ActionType expectedResultType)
        {
            _worm.Position = new Position(10, 10);
            _worm.Life = life;
            
            var result = _circlingWormBehaviour.GetAction(_state);
            Assert.That(result.Type, Is.EqualTo(expectedResultType));
        }
    }
}