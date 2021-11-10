using System.Collections.Generic;
using NUnit.Framework;

namespace NSU.Worm.Tests
{
    [TestFixture]
    public class NameGeneratorTest
    {
        private NameGenerator _nameGenerator;

        [SetUp]
        public void SetUp()
        {
            _nameGenerator = new NameGenerator();
        }

        [Test]
        public void TestNextName()
        {
            const string name = "Name";
            _nameGenerator.NamePool = new List<string> {name};

            var result = _nameGenerator.NextName();

            Assert.That(result, Is.EqualTo(name));
            Assert.Catch(() => _nameGenerator.NextName());
        }
    }
}