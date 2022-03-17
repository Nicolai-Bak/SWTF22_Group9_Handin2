using NUnit.Framework;

namespace Ladeskab.Test
{
    [TestFixture]
    public class TestStationControl
    {
        private StationControl _uut;
        [SetUp]
        public void Setup()
        {
            _uut = new StationControl();
        }

        [Test]
        public void Test1()
        {

            Assert.Pass();
        }

    }

}
