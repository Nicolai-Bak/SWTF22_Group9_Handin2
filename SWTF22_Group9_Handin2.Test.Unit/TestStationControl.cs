using NSubstitute;
using NUnit.Framework;
using SWTF22_Group9_Handin2_ClassLibrary;

namespace SWTF22_Group9_Handin2.Test.Unit;
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

    #region StateChanged tests
    #region Init
    [Test]
    public void InitState()
    {
        Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Available)));
    }
    #endregion

    #region DoorEvent
    [Test]
    public void DoorEvent_StateFromAvailableToDoorOpen()
    {
        // Arrange
        DoorEventArgs doorEventArgs = new DoorEventArgs(DoorStates.DoorOpen);

        // Act
        _door.DoorEvent += Raise.EventWith(doorEventArgs);

        // Assert
        Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(DoorOpen)));
    }

    [Test]
    public void DoorEvent_StateFromDoorOpenToAvailable()
    {
        // Arrange
        DoorEventArgs doorEventArgs = new DoorEventArgs(DoorStates.DoorOpen);
        _door.DoorEvent += Raise.EventWith(doorEventArgs);
        doorEventArgs = new DoorEventArgs(DoorStates.DoorClosed);

        // Act
        _door.DoorEvent += Raise.EventWith(doorEventArgs);

        // Assert
        Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Available)));
    }
    [Test]
    public void DoorEvent_StateStayDoorOpen_AlreadyOpen()
    {
        // Arrange
        DoorEventArgs doorEventArgs = new DoorEventArgs(DoorStates.DoorOpen);
        _door.DoorEvent += Raise.EventWith(doorEventArgs);
        // Act
        _door.DoorEvent += Raise.EventWith(doorEventArgs);

        // Assert
        Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(DoorOpen)));
    }
    [Test]
    public void DoorEvent_StateStayAvailable_AlreadyClosed()
    {
        // Arrange
        DoorEventArgs doorEventArgs = new DoorEventArgs(DoorStates.DoorClosed);
        // Act
        _door.DoorEvent += Raise.EventWith(doorEventArgs);

        // Assert
        Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Available)));
    }
    #endregion

    #region RfidEvent
    [Test]
    public void RfidEvent_StateFromAvailableToLocked()
    {
        // Arrange
        _chargeControl.IsConnected().Returns(true);
        RfidEventArgs rfidEventArgs = new RfidEventArgs { Id = 1 };
        // Act
        _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
        // Assert
        Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Locked)));
    }
    [Test]
    public void RfidEvent_StateFromLockedToAvailable_CorrectId()
    {
        // Arrange
        int id = 1;
        RfidEventArgs rfidEventArgs = new RfidEventArgs { Id = id };
        _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
        rfidEventArgs = new RfidEventArgs { Id = id };
        // Act
        _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
        // Assert
        Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Available)));
    }
    [Test]
    public void RfidEvent_StateStayAvailable_PhoneNotConnected()
    {
        // Arrange
        _chargeControl.IsConnected().Returns(false);
        RfidEventArgs rfidEventArgs = new RfidEventArgs { Id = 1 };
        // Act
        _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
        // Assert
        Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Available)));
    }
    [Test]
    public void RfidEvent_StateStayLocked_IncorrectId()
    {
        // Arrange
        _chargeControl.IsConnected().Returns(true);
        int id = 1;
        RfidEventArgs rfidEventArgs = new RfidEventArgs { Id = id };
        _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
        rfidEventArgs = new RfidEventArgs { Id = id + 1 };
        // Act
        _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
        // Assert
        Assert.That(_uut.State.GetType(), Is.EqualTo(typeof(Locked)));
    }
    #endregion

    #endregion

    #region Outgoing method calls Test

    [Test]
    public void Init()
    {
        // Assert
        _display.Received(1).DisplayMsg("Indlæs RFID");
    }
    [Test]
    public void DoorEvent_StateAvailable_DoorOpen()
    {
        // Arrange
        DoorEventArgs doorEventArgs = new DoorEventArgs(DoorStates.DoorOpen);
        // Act
        _door.DoorEvent += Raise.EventWith(doorEventArgs);

        // Assert
        _display.Received(1).DisplayMsg("Tilslut telefon");
    }
    [Test]
    public void DoorEvent_StateDoorOpen_DoorClosed()
    {
        // Arrange
        DoorEventArgs doorEventArgs = new DoorEventArgs(DoorStates.DoorOpen);
        _door.DoorEvent += Raise.EventWith(doorEventArgs);
        doorEventArgs = new DoorEventArgs(DoorStates.DoorClosed);
        // Act
        _door.DoorEvent += Raise.EventWith(doorEventArgs);

        // Assert
        _display.Received(2).DisplayMsg("Indlæs RFID");
    }
    [Test]
    public void RfidEvent_StateAvailable_PhoneNotConnected()
    {
        // Arrange
        _chargeControl.IsConnected().Returns(false);
        RfidEventArgs rfidEventArgs = new RfidEventArgs { Id = 1 };
        // Act
        _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
        // Assert
        _display.Received(1).DisplayMsg("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
    }
    [Test]
    public void RfidEvent_StateAvailable_PhoneConnected()
    {
        // Arrange
        _chargeControl.IsConnected().Returns(true);
        int id = 1;
        RfidEventArgs rfidEventArgs = new RfidEventArgs { Id = id };
        // Act
        _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
        // Assert
        _door.Received(1).LockDoor();
        _chargeControl.Received(1).StartCharging();
        _log.Received(1).LogDoorLocked(id);
        _display.Received(1).DisplayMsg("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
    }
    [Test]
    public void RfidEvent_StateLocked_IncorrectId()
    {
        // Arrange
        _chargeControl.IsConnected().Returns(true);
        int id = 1;
        RfidEventArgs rfidEventArgs = new RfidEventArgs { Id = id };
        _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
        rfidEventArgs = new RfidEventArgs { Id = id + 1 };
        // Act
        _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
        // Assert
        _display.Received(1).DisplayMsg("Forkert RFID tag");
    }
    [Test]
    public void RfidEvent_StateLocked_CorrectId()
    {
        // Arrange
        _chargeControl.IsConnected().Returns(true);
        int id = 1;
        RfidEventArgs rfidEventArgs = new RfidEventArgs { Id = id };
        _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
        rfidEventArgs = new RfidEventArgs { Id = id };
        // Act
        _rfidReader.RfidEvent += Raise.EventWith(rfidEventArgs);
        // Assert
        _chargeControl.Received(1).StopCharging();
        _door.Received(1).UnlockDoor();
        _log.Received(1).LogDoorUnlocked(id);
        _display.Received(1).DisplayMsg("Tag din telefon ud af skabet og luk døren");
    }
    #endregion
}
