using NUnit.Framework;

namespace DND.Data.Test
{
    [TestFixture]
    public class CardDataAccessTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetCard(int id)
        {
            Assert.Pass();
        }
    }
}