using System;
using SWTF22_Group9_Handin2_ClassLibrary;

namespace UsbSimulator
{
    public interface IChargeControl
    {
        void StartCharging();

        void StopCharging();

        bool IsConnected();

        void OnCurrentEvent(object o, CurrentEventArgs args);
    }

    public class ChargeControl : IChargeControl
    {
        private IUsbCharger _usbCharger;
        private IDisplay _display;

        public ChargeControl(IUsbCharger usbCharger, IDisplay display)
        {
            _usbCharger = usbCharger;
            _display = display;
            usbCharger.CurrentValueEvent += OnCurrentEvent;
        }

        public void StartCharging()
        {
            _usbCharger.StartCharge();
            _display.DisplayMsg("Enheden oplader");
        }

        public void StopCharging()
        {
            _usbCharger.StopCharge();
        }

        public bool IsConnected()
        {
            if (_usbCharger.Connected)
                return true;
            else
                return false;
        }

        public void OnCurrentEvent(object o, CurrentEventArgs args)
        {
            if (args.Current == 0)
            {
                StopCharging();
            }
            else if (args.Current <= 5)
            {
                StopCharging();
                _display.DisplayMsg("Enheden er fuldt opladt");
            }
            else if (args.Current > 5)
            {
                if (args.Current <= 500)
                {
                    StartCharging();
                }
                else
                {
                    StopCharging();
                    _display.DisplayMsg("ERROR40: KAN IKKE OPLADES KORREKT");
                }
            }
        }
    }
}
