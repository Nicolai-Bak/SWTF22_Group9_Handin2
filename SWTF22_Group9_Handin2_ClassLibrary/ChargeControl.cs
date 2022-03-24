namespace SWTF22_Group9_Handin2_ClassLibrary;

public class ChargeControl : IChargeControl
{
    private IUsbCharger _usbCharger;

    private IDisplay _display;

    public ChargeControl(IUsbCharger usbCharger, IDisplay display)
    {
        _usbCharger = usbCharger;
        _display = display;
        _usbCharger.CurrentValueEvent += OnCurrentEvent;
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
        if(_usbCharger.Connected)
            return true;
        else
            return false;
    }

    public void OnCurrentEvent(object o, CurrentEventArgs args)
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