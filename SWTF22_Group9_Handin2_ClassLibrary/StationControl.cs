namespace SWTF22_Group9_Handin2_ClassLibrary;

public class StationControl
{
    public int OldId { get; set; }
    public State State { get; set; }
    public IChargeControl Charger { get; set; }
    public IDoor Door { get; set; }
    public ILog Log { get; set; }
    public IDisplay Display { get; set; }

    public StationControl(IChargeControl charger, IDoor door, ILog log, IDisplay display, IRfidReader rfidReader)
    {
        Charger = charger;
        Door = door;
        Log = log;
        Display = display;

        State = new Available();
        OldId = 0;

        //Door.UnlockDoor();
        Display.DisplayMsg("Indlæs RFID");

        rfidReader.RfidEvent += OnRfidEvent!;
        door.DoorEvent += OnDoorEvent!;
    }


    // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
    private void OnRfidEvent(object o, RfidEventArgs args)
    {
        State.HandleRfidEvent(this, args);
    }
    //
    //         // Her mangler de andre trigger handlere
    private void OnDoorEvent(object o, DoorEventArgs args)
    {
        State.HandleDoorEvent(this, args);
    }
}

