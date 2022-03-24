using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;
using SWTF22_Group9_Handin2_ClassLibrary;
using NSubstitute;

namespace SWTF22_Group9_Handin2.Test.Unit
{
    [TestFixture]
    public class TestDoorSimulator
    {
        private DoorSimulator _uut;
        [SetUp]
        public void Setup()
        {
            _uut = new DoorSimulator();
        }

        // Test of UnlockDoor
        [Test]
        public void UnlockDoorWhileLocked()
        {
            _uut.doorstate = DoorStates.DoorLocked;
            _uut.UnlockDoor();
            Assert.That(_uut.doorstate == DoorStates.DoorClosed); 
        }

        [Test]
        public void UnlockDoorWhileNotLocked()
        {
            _uut.doorstate = DoorStates.DoorOpen;
            _uut.UnlockDoor();
            Assert.That(_uut.doorstate == DoorStates.DoorOpen);
        }


        //Test of LockDoor

        [Test]
        public void LockDoorWhileClosed()
        {
            _uut.doorstate = DoorStates.DoorClosed;
            _uut.LockDoor();
            Assert.That(_uut.doorstate == DoorStates.DoorLocked);
        }

        [Test]
        public void LockDoorWhileOpen()
        {
            _uut.doorstate = DoorStates.DoorOpen;
            _uut.LockDoor();
            Assert.That(_uut.doorstate == DoorStates.DoorOpen);
        }



        //Test of OnDoorOpen

        [Test]
        public void OnDoorOpenWhileClosed()
        {
            _uut.doorstate = DoorStates.DoorClosed;
            _uut.OnDoorOpen();
            Assert.That(_uut.doorstate == DoorStates.DoorOpen);

        }

      
        [Test]
        public void OnDoorOpenWhileOpen()
        {

            List<DoorStates> receivedEvents = new List<DoorStates>();

            _uut.DoorEvent += delegate (object sender, DoorEventArgs e)
            {
                receivedEvents.Add(e.States);
            };

            _uut.doorstate = DoorStates.DoorOpen;
            _uut.OnDoorOpen();

            Assert.AreEqual(0, receivedEvents.Count);
            Assert.That(_uut.doorstate == DoorStates.DoorOpen);

        }


        //Test of OnDoorClose

        [Test]
        public void OnDoorCloseWhileOpen()
        {
            _uut.doorstate = DoorStates.DoorOpen;
            _uut.OnDoorClose();
            Assert.That(_uut.doorstate == DoorStates.DoorClosed);

        }


        [Test]
        public void OnDoorCloseWhileClosed()
        {

            List<DoorStates> receivedEvents = new List<DoorStates>();

            _uut.DoorEvent += delegate (object sender, DoorEventArgs e)
            {
                receivedEvents.Add(e.States);
            };

            _uut.doorstate = DoorStates.DoorClosed;
            _uut.OnDoorClose();

            Assert.AreEqual(0, receivedEvents.Count);
            Assert.That(_uut.doorstate == DoorStates.DoorClosed);

        }




        //Test of DoorChangedEvent

        [Test]
        public void DoorChangedEvent()
        {

            List<DoorStates> receivedEvents = new List<DoorStates>();

            _uut.DoorEvent += delegate (object sender, DoorEventArgs e)
            {
                receivedEvents.Add(e.States);
            };

            _uut.doorstate = DoorStates.DoorClosed;
            _uut.OnDoorOpen();
            _uut.doorstate = DoorStates.DoorOpen;
            _uut.OnDoorClose();

            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual(DoorStates.DoorOpen, receivedEvents[0]);
            Assert.AreEqual(DoorStates.DoorClosed, receivedEvents[1]);

        }



    }
}

