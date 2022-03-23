using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using SWTF22_Group9_Handin2_ClassLibrary;
using UsbSimulator;

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
            _rfidReader = Substitute.For<IRfidReader>();

            _uut = new StationControl(_chargeControl, _door, _log, _display, _rfidReader);
        }

        [TestCase(10, true)]
        public void RfidEvent_StateAvailable_PhoneConnected(int id, bool connected)
        {
            // Arrange
            State state = new Available();
            _uut.State = state;

            RfidEventArgs rfidEventArgs = new RfidEventArgs {Id = id};

            // Act
            _chargeControl.Connected.Returns(connected);
            _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
            
            // Assert
            state.Received(1).HandleRfidEvent(_uut, rfidEventArgs);
            Assert.That(_uut.OldId, Is.EqualTo(id));
            _door.Received(1).LockDoor();
            _chargeControl.Received(1).StartCharging();
            _log.Received(1).LogDoorLocked(id);
            _display.Received(1).DisplayMsg("Skabet er låst og din telefon lades.Brug dit RFID tag til at låse op.");
            Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Locked)));
        }

        [TestCase(10,false)]
        public void RfidEvent_StateAvailable_PhoneNotConnected(int id, bool connected)
        {
            //Assert: display kaldt, state uændret
            // Arrange
            State state = new Available();
            _uut.State = state;

            RfidEventArgs rfidEventArgs = new RfidEventArgs { Id = id };

            // Act
            _chargeControl.Connected.Returns(connected);
            _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);


            // Assert
            state.Received(1).HandleRfidEvent(_uut, rfidEventArgs);
            _door.Received(0).LockDoor();
            _chargeControl.Received(0).StartCharging();
            _log.Received(0).LogDoorLocked(id);
            _display.Received(1).DisplayMsg("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
            Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Available)));
        }

        [TestCase(10,10)]
        public void RfidEvent_StateLocked_IdCorrect(int oldId, int id)
        {
            //Assert: stopcharging kaldt, unlockdoor kaldt, log kaldt, display kaldt
            // Arrange
            State state = new Locked();
            _uut.State = state;

            _uut.OldId = oldId;

            RfidEventArgs rfidEventArgs = new RfidEventArgs { Id = id };

            // Act
            _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);

            // Assert
            state.Received(1).HandleRfidEvent(_uut, rfidEventArgs);
            _chargeControl.Received(1).StopCharging();
            _door.Received(1).UnlockDoor();
            _log.Received(1).LogDoorUnlocked(id);
            _display.Received(1).DisplayMsg("Tag din telefon ud af skabet og luk døren");
            Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Available)));
        }

        [TestCase(10,14)]
        public void RfidEvent_StateLocked_IdIncorrect(int oldId,int id)
        {
            //Assert: dislpay kaldt
            // Arrange
            State state = new Locked();
            _uut.State = state;

            _uut.OldId = oldId;

            RfidEventArgs rfidEventArgs = new RfidEventArgs { Id = id };

            // Act
            _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);

            // Assert
            state.Received(1).HandleRfidEvent(_uut, rfidEventArgs);
            _chargeControl.Received(1).StopCharging();
            _door.Received(0).UnlockDoor();
            _log.Received(0).LogDoorUnlocked(id);
            _display.Received(1).DisplayMsg("Forkert RFID tag");
            Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Locked)));
        }

        [TestCase(10)]
        public void RfidEvent_StateDoorOpen_NoAction(int id)
        {
            //Assert: der skal ikke ske noget
            // Arrange
            State state = new DoorOpen();
            _uut.State = state;

            RfidEventArgs rfidEventArgs = new RfidEventArgs { Id = id };

            // Act
            _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);

            // Assert
            state.Received(1).HandleRfidEvent(_uut, rfidEventArgs);
            Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(DoorOpen)));
        }

        [TestCase(true)]
        public void DoorEvent_StateAvailable(bool isOpen)
        {
            //Assert: dislay kaldt
            // Arrange
            State state = new Available();
            _uut.State = state;

            DoorEventArgs doorEventArgs = new DoorEventArgs { IsOpen = isOpen };

            // Act
            _door.DoorEvent += Raise.EventWith(doorEventArgs);

            // Assert
            state.Received(1).HandleDoorEvent(_uut, doorEventArgs);
            _display.Received(1).DisplayMsg("Tilslut telefon");
            Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(DoorOpen)));
        }

        //[TestCase(true)]
        //public void DoorEvent_StateLocked(bool isOpen)
        //{
        //    //Assert: der skal ikke ske noget
        //    // Arrange
        //    State state = new Locked();
        //    _uut.State = state;

        //    DoorEventArgs doorEventArgs = new DoorEventArgs { IsOpen = isOpen };

        //    // Act
        //    _door.DoorEvent += Raise.EventWith(doorEventArgs);

        //    // Assert
        //    state.Received(1).HandleDoorEvent(_uut, doorEventArgs);
        //    Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Locked)));
        //}

        [TestCase(false)]
        public void DoorEvent_StateDoorOpen(bool isOpen)
        {
            //Assert: display kaldt
            // Arrange
            State state = new DoorOpen();
            _uut.State = state;

            DoorEventArgs doorEventArgs = new DoorEventArgs { IsOpen = isOpen };

            // Act
            _door.DoorEvent += Raise.EventWith(doorEventArgs);

            // Assert
            state.Received(1).HandleDoorEvent(_uut, doorEventArgs);
            _display.Received(1).DisplayMsg("Indlæs RFID");
            Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Available)));
        }
    }
}
