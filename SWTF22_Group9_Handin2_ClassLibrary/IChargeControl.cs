namespace SWTF22_Group9_Handin2_ClassLibrary
{
    public interface IChargeControl
    {
        void StartCharging();

        void StopCharging();

        bool isConnected();

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

        public bool isConnected()
        {
            if(_usbCharger.Connected)
                return true;
            else
                return false;
        }

        void IChargeControl.OnCurrentEvent(object o, CurrentEventArgs args)
        {
            switch(args.Current)
            {
            case 0:
                    this.StopCharging();
                    break;
            case <= 5:
                    StopCharging();
                    _display.DisplayMsg("Enheden er fuldt opladt");
                    break;
            case > 5:
                    if(args.Current <= 500)
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
