namespace SWTF22_Group9_Handin2_ClassLibrary
{
    public interface IChargeControl
    {
        void StartCharging();

        void StopCharging();

        bool IsConnected();

        void OnCurrentEvent(object o, CurrentEventArgs args);
    }
}
