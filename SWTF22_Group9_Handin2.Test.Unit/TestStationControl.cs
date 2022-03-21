using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using NSubstitute;
using NUnit.Framework;
using SWTF22_Group9_Handin2_ClassLibrary;

namespace SWTF22_Group9_Handin2.Test.Unit
{
    [TestFixture]
    public class TestStationControl
    {
        private StationControl _uut;

        private IChargeControl _chargeControl;
        private IDoor _door;
        private ILog _log;
        private IDisplay _display;
        private IRfidReader _rfidReader;


        [SetUp]
        public void Setup()
        {
            _chargeControl = Substitute.For<IChargeControl>();
            _door = Substitute.For<IDoor>();
            _log = Substitute.For<ILog>();
            _display = Substitute.For<IDisplay>();

            _uut = new StationControl(_chargeControl, _door, _log, _display);

            _rfidReader = Substitute.For<IRfidReader>();
        }

        /* Scenarier:
        RfidEvent - Locked - Korrekt ID - Assert: 
        RfidEvent - Locked - Korrekt ID - Assert: 
         
         */

        [TestCase(10)]
        public void RfidEvent_StateAvailable_PhoneConnected(int id)
        {
            //Assert: tjek oldid, lockdoor kaldt, startcharging kaldt, log kaldt, state opdatert til locked, display kaldt
            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs {Id = id});
            Assert.Pass();
        }
        [TestCase(10)]
        public void RfidEvent_StateAvailable_PhoneNotConnected(int id)
        {
            //Assert: display kaldt, state uændret
            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs { Id = id });
            Assert.Pass();
        }
        [TestCase(10)]
        public void RfidEvent_StateLocked_IdCorrect(int id)
        {
            //Assert: stopcharging kaldt, unlockdoor kaldt, log kaldt, display kaldt
            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs { Id = id });
            Assert.Pass();
        }
        [TestCase(10)]
        public void RfidEvent_StateLocked_IdIncorrect(int id)
        {
            //Assert: dislpay kaldt
            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs { Id = id });
            Assert.Pass();
        }
        [TestCase(10)]
        public void RfidEvent_StateDoorOpen_NoAction(int id)
        {
            //Assert: der skal ikke ske noget
            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs { Id = id });
            Assert.Pass();
        }
        [TestCase(10)]
        public void DoorEvent_StateAvailable(int id)
        {
            //Assert: dislay kaldt
            _door.DoorEvent += Raise.EventWith(new DoorEventArgs { Id = id });
            Assert.Pass();
        }
        [TestCase(10)]
        public void DoorEvent_StateLocked(int id)
        {
            //Assert: display kaldt
            _door.DoorEvent += Raise.EventWith(new DoorEventArgs { Id = id });
            Assert.Pass();
        }
        [TestCase(10)]
        public void DoorEvent_StateDoorOpen(int id)
        {
            //Assert: der skal ikke ske noget
            _door.DoorEvent += Raise.EventWith(new DoorEventArgs { Id = id });
            Assert.Pass();
        }
    }

}
