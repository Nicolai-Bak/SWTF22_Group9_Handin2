namespace SWTF22_Group9_Handin2_ClassLibrary;

public class Available : State
{
    public override void HandleRfidEvent(StationControl s, RfidEventArgs a)
    {
        if (s.Charger.IsConnected())
        {
            s.Door.LockDoor();
            s.Charger.StartCharging();
            s.OldId = a.Id;
            s.Log.LogDoorLocked(a.Id);
            s.Display.DisplayMsg("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
            s.State = new Locked();
        }
        else
        {
            s.Display.DisplayMsg("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
        }
    }
    public override void HandleDoorEvent(StationControl s, DoorEventArgs a)
    {
        if (a.States == DoorStates.DoorOpen)
        {
            s.Display.DisplayMsg("Tilslut telefon");
            s.State = new DoorOpen();
        }
        else if (a.States == DoorStates.DoorClosed)
        {
            s.Display.DisplayMsg("Fejl: Lukket dør blev lukket");
        }
    }
}