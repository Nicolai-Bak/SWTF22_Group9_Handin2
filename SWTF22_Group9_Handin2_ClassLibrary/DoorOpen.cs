namespace SWTF22_Group9_Handin2_ClassLibrary;

public class DoorOpen : State
{
    public override void HandleDoorEvent(StationControl s, DoorEventArgs a)
    {
        if (a.States == DoorStates.DoorClosed)
        {
            s.Display.DisplayMsg("Indlæs RFID");
            s.State = new Available();

        }
        else if (a.States == DoorStates.DoorOpen)
        {
            s.Display.DisplayMsg("Fejl: Åben dør blev åbnet");
        }
        else
        {
            s.Display.DisplayMsg("Fejl: Åben dør blev låst");
        }
    }
}