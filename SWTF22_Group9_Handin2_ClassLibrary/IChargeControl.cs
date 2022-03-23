using System;

namespace UsbSimulator
{
    public interface IChargeControl
    {
        void StartCharging();

        void StopCharging();

        bool isConnected();

        void OnCurrentEvent(object o, CurrentEventsArg args);
    }

    public class ChargeControl : IChargeControl
    {
        private IUSBCharger _usbCharger;

        private IDisplay _display;

        public ChargeControl(IUSBCharger usbCharger, IDisplay display)
        {
            _usbCharger = usbCharger;
            _display = display;
        }

        void IChargeControl.StartCharging()
        {
            _usbCharger.StartCharge();
            _display.DisplayMsg("Enheden oplader");
        }

        void IChargeControl.StopCharging()
        {
            _usbCharger.StopCharge();
        }

        bool IChargeControl.isConnected()
        {
            if (_usbCharger.Connected)
                return true;
            else
                return false;
        }

        void IChargeControl.OnCurrentEvent(object o, CurrentEventsArg args)
        {
            switch (args.Current)
            {
                case 0:
                    StopCharging();
                    break;
                case <= 5:
                    StopCharging();
                    _display.DisplayMsg("Enheden er fuldt opladt");
                    break;
                case > 5:
                    if (args.Current <= 500)
                    {
                        StartCharging();
                        break;
                    }
                    else
                    {
                        StopCharging();
                        _display.DisplayMsg("ERROR40: KAN IKKE OPLADES KORREKT");
                        break;
                    }
            }
        }
    }
}
