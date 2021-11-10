using NUnit.Framework;

namespace NSU.Worm.Tests
{
    [TestFixture]
    public class WormBehaviourProviderTest
    {
        private WormBehaviourProvider _wormBehaviourProvider;

        [SetUp]
        public void SetUp()
        {
            _wormBehaviourProvider = new WormBehaviourProvider();
        }

        [Test]
        public void TestRegisterAndGetBehaviour()
        {
            var worm = new Worm("Name", 10, 1, 1);
            var behaviour = new CirclingWormBehaviour(worm, worm.Position);

            _wormBehaviourProvider.RegisterBehaviour(worm, behaviour);
            var result = _wormBehaviourProvider.GetBehaviour(worm);

            Assert.That(result, Is.EqualTo(behaviour));
        }

        [Test]
        public void TestTryRegisterTwice()
        {
            var worm = new Worm("Name", 10, 1, 1);
            var behaviour1 = new CirclingWormBehaviour(worm, worm.Position);
            var behaviour2 = new CirclingWormBehaviour(worm, worm.Position);

            _wormBehaviourProvider.RegisterBehaviour(worm, behaviour1);

            Assert.Catch(() => _wormBehaviourProvider.RegisterBehaviour(worm, behaviour2));
        }

        [Test]
        public void TestRegisterAndDeleteBehaviour()
        {
            var worm = new Worm("Name", 10, 1, 1);
            var behaviour = new CirclingWormBehaviour(worm, worm.Position);

            _wormBehaviourProvider.RegisterBehaviour(worm, behaviour);
            _wormBehaviourProvider.DeleteBehaviour(worm);

            Assert.Catch(() => _wormBehaviourProvider.GetBehaviour(worm));
        }
    }
}