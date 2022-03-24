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
    public virtual void HandleRfidEvent(StationControl stationControl, RfidEventArgs args) { }
    public virtual void HandleDoorEvent(StationControl stationControl, DoorEventArgs args) { }
}

public class Available : State
{
    public override void HandleRfidEvent(StationControl stationControl, RfidEventArgs args)
    {
        if (stationControl.Charger.isConnected())
        {
            stationControl.Door.LockDoor();
            stationControl.Charger.StartCharging();
            stationControl.OldId = args.Id;
            stationControl.Log.LogDoorLocked(args.Id);
            stationControl.Display.DisplayMsg("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
            stationControl.State = new Locked();
        }
        else
        {
            stationControl.Display.DisplayMsg("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
        }
    }
    public override void HandleDoorEvent(StationControl stationControl, DoorEventArgs args)
    {
        if (args.States == DoorStates.DoorOpen)
        {
            stationControl.Display.DisplayMsg("Tilslut telefon");
            stationControl.State = new DoorOpen();
        }
        else if (args.States == DoorStates.DoorClosed)
        {
            stationControl.Display.DisplayMsg("Fejl: Lukket dør blev lukket");
        }
    }
}
public class Locked : State
{
    public override void HandleRfidEvent(StationControl stationControl, RfidEventArgs args)
    {
        if (args.Id == stationControl.OldId)
        {
            stationControl.Charger.StopCharging();
            stationControl.Door.UnlockDoor();
            stationControl.Log.LogDoorUnlocked(args.Id);
            stationControl.Display.DisplayMsg("Tag din telefon ud af skabet og luk døren");
            stationControl.State = new Available();
        }
        else
        {
            stationControl.Display.DisplayMsg("Forkert RFID tag");
        }
    }
}
public class DoorOpen : State
{
    public override void HandleDoorEvent(StationControl stationControl, DoorEventArgs args)
    {
        if (args.States == DoorStates.DoorClosed)
        {
            stationControl.Display.DisplayMsg("Indlæs RFID");
            stationControl.State = new Available();

        }
        else if (args.States == DoorStates.DoorOpen)
        {
            stationControl.Display.DisplayMsg("Fejl: Åben dør blev åbnet");
        }
        else
        {
            stationControl.Display.DisplayMsg("Fejl: Åben dør blev låst");
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

