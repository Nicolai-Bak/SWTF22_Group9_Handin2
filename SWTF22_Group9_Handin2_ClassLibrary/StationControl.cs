using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWTF22_Group9_Handin2_ClassLibrary;
using UsbSimulator;

namespace SWTF22_Group9_Handin2_ClassLibrary;

public abstract class State
{
    public virtual void HandleRfidEvent(StationControl s, RfidEventArgs a) { }
    public virtual void HandleDoorEvent(StationControl s, DoorEventArgs a) { }
}

public class Available : State
{
    public override void HandleRfidEvent(StationControl s, RfidEventArgs a)
    {
        if (s.Charger.isConnected())
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

