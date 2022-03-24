using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SWTF22_Group9_Handin2_ClassLibrary;


namespace SWTF22_Group9_Handin2.Test.Unit
{

    [TestFixture]
    public class TestChargeControl
    {
        private ChargeControl _uut;
        private IUsbCharger _usbCharger;
        private IDisplay _display;

        [SetUp]
        public void Setup()
        {
            _usbCharger = Substitute.For<IUsbCharger>();
            _display = Substitute.For<IDisplay>();
            _uut = new ChargeControl(_usbCharger, _display);
        }

        [Test]
        public void StartCharging_MethhodCallToUsbChargerAndDisplay()
        {
            _uut.StartCharging();
            _usbCharger.Received(1).StartCharge();
            _display.Received(1).DisplayMsg("Enheden oplader");
        }

        [Test]
        public void StopCharging_MethhodCallToUsbCharger()
        {
            _uut.StopCharging();
            _usbCharger.Received(1).StopCharge();
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void IsConnected_CallsUsbChargerMethodConnectedAndReturnsTrue(bool input, bool output)
        {
            _usbCharger.Connected.Returns(input);
            
            Assert.That(_uut.IsConnected(), Is.EqualTo(output));
        }        

        [Test]
        
        public void OnCurrentEvent_CurrentIsZero()
        {
            CurrentEventArgs currentEventArgs = new CurrentEventArgs { Current = 0};

            _usbCharger.CurrentValueEvent += Raise.EventWith(currentEventArgs); //opret event

            _usbCharger.Received(1).StopCharge(); //check om der kaldes til StopCharging, hvis Current = 0
        }

        [Test]
        public void OnCurrentEvent_CurrentIsFive()
        {
            CurrentEventArgs currentEventArgs = new CurrentEventArgs { Current = 5 };

            _usbCharger.CurrentValueEvent += Raise.EventWith(currentEventArgs); //opret event

            _usbCharger.Received(1).StopCharge(); //check om der kaldes til StopCharging, hvis Current <= 5

            _display.Received(1).DisplayMsg("Enheden er fuldt opladt");
        }

        [Test]
        public void OnCurrentEvent_CurrentIsOneHundred()
        {
            CurrentEventArgs currentEventArgs = new CurrentEventArgs { Current = 100 };

            _usbCharger.CurrentValueEvent += Raise.EventWith(currentEventArgs); //opret event

            _usbCharger.Received(1).StartCharge(); 
        }

        [Test]
        public void OnCurrentEvent_CurrentIsSixHundred()
        {
            CurrentEventArgs currentEventArgs = new CurrentEventArgs { Current = 600 };

            _usbCharger.CurrentValueEvent += Raise.EventWith(currentEventArgs); //opret event

            _usbCharger.Received(1).StopCharge(); //check om der kaldes til StopCharging, hvis Current < 500

            _display.Received(1).DisplayMsg("ERROR40: KAN IKKE OPLADES KORREKT");
        }
    }
}