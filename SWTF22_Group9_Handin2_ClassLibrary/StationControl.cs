using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ladeskab;

namespace Ladeskab
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        private enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        //         // Her mangler flere member variable
        private LadeskabState _state;
        private int _oldId;
        private bool _isDoorOpen;
        private bool _isDoorLocked;

        private IChargeControl _charger;
        private IDoor _door;
        private ILog _log;
        private IDisplay _display;

        public StationControl(IChargeControl charger,
                              IDoor door,
                              ILog log,
                              IDisplay display)
        {
            _charger = charger;
            _door = door;
            _log = log;
            _display = display;

            _door.UnlockDoor();
            _display.DisplayMsg("Indlæs RFID");
        }


        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        private void OnRfidEvent(RfidEventArgs args)
        {
            switch (_state)
            {
                case LadeskabState.Available:
                    // Check for ladeforbindelse
                    if (_charger.Connected)
                    {
                        _door.LockDoor();
                        _charger.StartCharging();
                        _oldId = args.Id;
                        _log.LogDoorLocked(args.Id);
                        _display.DisplayMsg("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                        _state = LadeskabState.Locked;
                    }
                    else
                    {
                        _display.DisplayMsg("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }
                    break;

                case LadeskabState.DoorOpen:
                    // Ignore
                    break;

                case LadeskabState.Locked:
                    // Check for correct ID
                    if (args.Id == _oldId)
                    {
                        _charger.StopCharging();
                        _door.UnlockDoor();
                        _log.LogDoorUnlocked(args.Id);
                        _display.DisplayMsg("Tag din telefon ud af skabet og luk døren");
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        _display.DisplayMsg("Forkert RFID tag");
                    }
                    break;
            }
        }
        //
        //         // Her mangler de andre trigger handlere
        private void OnDoorEvent(DoorEventArgs args)
        {
            switch (_state)
            {
                case LadeskabState.Available:
                    if (args.IsOpen)
                    {
                        _display.DisplayMsg("Tilslut telefon");
                    }
                    else
                    {
                        _display.DisplayMsg("Indlæs RFID");
                    }
                    break;

                case LadeskabState.Locked:

                    break;

                case LadeskabState.DoorOpen:
                    break;

            }
        }
    }
}
