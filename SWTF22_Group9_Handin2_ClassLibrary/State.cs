namespace SWTF22_Group9_Handin2_ClassLibrary;

public abstract class State
{
    public virtual void HandleRfidEvent(StationControl s, RfidEventArgs a) { }
    public virtual void HandleDoorEvent(StationControl s, DoorEventArgs a) { }
}