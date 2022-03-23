using NUnit.Framework;
using SWTF22_Group9_Handin2_ClassLibrary;

namespace SWTF22_Group9_Handin2.Test.Unit
{
    [TestFixture]
    public class TestRfidReaderSimulator
    {
        private RfidReaderSimulator _uut;
        [SetUp]
        public void Setup()
        {
            _uut = new RfidReaderSimulator();
        }

        [Test]
        public void SimulateRead_EventTriggered()
        {
            bool checkTrigger = false;
            _uut.RfidEvent += ((sender, args) => checkTrigger = true);

            _uut.OnRfidRead(1);

            Assert.IsTrue(checkTrigger);
        }

        [Test]
        public void SimulateRead_EventArgsMatchIdRead()
        {
            int sendId = 123;
            int checkId = 456;
            _uut.RfidEvent += ((sender, args) => checkId = args.Id);

            _uut.OnRfidRead(sendId);

            Assert.That(sendId, Is.EqualTo(checkId));
        }
    }
}