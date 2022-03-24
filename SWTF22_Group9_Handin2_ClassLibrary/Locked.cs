namespace SWTF22_Group9_Handin2_ClassLibrary;

public class Locked : State
{
    public override void HandleRfidEvent(StationControl s, RfidEventArgs a)
    {
        if (a.Id == s.OldId)
        {
            s.Charger.StopCharging();
            s.Door.UnlockDoor();
            s.Log.LogDoorUnlocked(a.Id);
            s.Display.DisplayMsg("Tag din telefon ud af skabet og luk døren");
            s.State = new Available();
        }
        else
        {
            s.Display.DisplayMsg("Forkert RFID tag");
        }
    }
}