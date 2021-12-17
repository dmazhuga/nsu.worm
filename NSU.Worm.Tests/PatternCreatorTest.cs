using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSU.Worm.CreateFoodPattern;
using NSU.Worm.Data;
using NSubstitute;
using NUnit.Framework;

namespace NSU.Worm.Tests
{
    [TestFixture]
    public class PatternCreatorTest
    {
        private const string PatternName = "name";
        private const int Iterations = 2;

        private const int PositionX1 = 0;
        private const int PositionY1 = 1;
        private const int PositionX2 = -2;
        private const int PositionY2 = -3;

        private const int Freshness = 99;

        private PatternCreator _patternCreator;

        private IFoodGenerator _randomFoodGenerator;

        private EnvironmentContext _context;

        [SetUp]
        public void SetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<EnvironmentContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new EnvironmentContext(dbOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _randomFoodGenerator = Substitute.For<IFoodGenerator>();
            _patternCreator = new PatternCreator(_randomFoodGenerator);
        }

        [Test]
        public void TestCreateAndSaveRandom()
        {
            var foodStep1 = new Food(PositionX1, PositionY1, Freshness);
            var foodStep2 = new Food(PositionX2, PositionY2, Freshness);
            _randomFoodGenerator.GenerateFood(default).ReturnsForAnyArgs(foodStep1, foodStep2);

            _patternCreator.CreateAndSaveRandom(PatternName, Iterations, _context);

            _randomFoodGenerator.Received().GenerateFood(Arg.Any<int>());

            var resultPattern = _context.Patterns
                .Where(p => p.Name == PatternName)
                .Include(p => p.Steps)
                .Single();
            var resultStep1 = resultPattern.Steps.Single(s => s.Iteration == 0);
            var resultStep2 = resultPattern.Steps.Single(s => s.Iteration == 1);

            Assert.That(resultPattern.Iterations, Is.EqualTo(Iterations));
            Assert.That(resultPattern.Steps.Count, Is.EqualTo(Iterations));
            Assert.That(resultStep1.PositionX, Is.EqualTo(PositionX1));
            Assert.That(resultStep1.PositionY, Is.EqualTo(PositionY1));
            Assert.That(resultStep2.PositionX, Is.EqualTo(PositionX2));
            Assert.That(resultStep2.PositionY, Is.EqualTo(PositionY2));
        }

        [Test]
        public void TestCreateAndSaveFromPositionList()
        {
            var position1 = new Position(PositionX1, PositionY1);
            var position2 = new Position(PositionX2, PositionY2);
            var positionList = new List<Position> {position1, position2};
            
            _patternCreator.CreateAndSaveFromPositionList(PatternName, Iterations, positionList, _context);

            var resultPattern = _context.Patterns
                .Where(p => p.Name == PatternName)
                .Include(p => p.Steps)
                .Single();
            var resultStep1 = resultPattern.Steps.Single(s => s.Iteration == 0);
            var resultStep2 = resultPattern.Steps.Single(s => s.Iteration == 1);

            Assert.That(resultPattern.Iterations, Is.EqualTo(Iterations));
            Assert.That(resultPattern.Steps.Count, Is.EqualTo(Iterations));
            Assert.That(resultStep1.PositionX, Is.EqualTo(PositionX1));
            Assert.That(resultStep1.PositionY, Is.EqualTo(PositionY1));
            Assert.That(resultStep2.PositionX, Is.EqualTo(PositionX2));
            Assert.That(resultStep2.PositionY, Is.EqualTo(PositionY2));
        }
    }
}