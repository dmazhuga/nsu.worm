using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSU.Worm.Data;
using NSubstitute;
using NUnit.Framework;

namespace NSU.Worm.Tests
{
    [TestFixture]
    public class PatternFoodGeneratorTest
    {
        private const string PatternName = "name";
        private const int Iterations = 2;

        private readonly FoodGenerationPatternStep Step1 = new()
            {Iteration = 0, PositionX = 0, PositionY = 0};

        private readonly FoodGenerationPatternStep Step2 = new()
            {Iteration = 1, PositionX = 1, PositionY = 1};

        private const int FoodFreshness = 99;

        private PatternFoodGenerator _patternFoodGenerator;

        [SetUp]
        public void SetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<EnvironmentContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var contextFactory = new TestDbContextFactory(dbOptions);

            using var context = contextFactory.CreateDbContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var pattern = new FoodGenerationPattern
            {
                Name = PatternName, Iterations = Iterations, Steps = new List<FoodGenerationPatternStep> {Step1, Step2}
            };
            
            context.Patterns.Add(pattern);
            context.SaveChanges();
            
            var simOptions = Substitute.For<IOptions<SimulatorOptions>>();
            simOptions.Value.Returns(new SimulatorOptions
            {
                FoodPatternName = PatternName
            });
            
            _patternFoodGenerator = new PatternFoodGenerator(simOptions, contextFactory);
        }

        [Test]
        public void TestGenerateFood()
        {
            var food1 = _patternFoodGenerator.GenerateFood(FoodFreshness);
            var food2 = _patternFoodGenerator.GenerateFood(FoodFreshness);
            
            Assert.That(food1.Position.X, Is.EqualTo(Step1.PositionX));
            Assert.That(food1.Position.Y, Is.EqualTo(Step1.PositionY));
            Assert.That(food2.Position.X, Is.EqualTo(Step2.PositionX));
            Assert.That(food2.Position.Y, Is.EqualTo(Step2.PositionY));
        }
    }

    internal class TestDbContextFactory : IDbContextFactory<EnvironmentContext>
    {
        private DbContextOptions<EnvironmentContext> _options;

        public TestDbContextFactory(DbContextOptions<EnvironmentContext> options)
        {
            _options = options;
        }

        public EnvironmentContext CreateDbContext()
        {
            return new EnvironmentContext(_options);
        }
    }
}