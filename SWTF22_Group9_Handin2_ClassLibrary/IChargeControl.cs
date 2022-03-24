using System;

namespace UsbSimulator
{
    public interface IChargeControl
    {
        void StartCharging();

        void StopCharging();

        bool isConnected();

        void OnCurrentEvent(object o, CurrentEventArgs args);
    }
}
