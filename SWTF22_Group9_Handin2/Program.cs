// See https://aka.ms/new-console-template for more information

using SWTF22_Group9_Handin2_ClassLibrary;
using UsbSimulator;

UsbChargerSimulator usbCharger = new UsbChargerSimulator();
DoorSimulator door = new DoorSimulator();
RfidReaderSimulator rfidReader = new RfidReaderSimulator();
DisplaySimulator display = new DisplaySimulator();
ChargeControl chargeControl = new ChargeControl(usbCharger, display);
LogToTxt log = new LogToTxt();

StationControl stationControl = new StationControl(chargeControl, door, log, display, rfidReader);

bool finish = false;

do
{
    string input;
    System.Console.WriteLine("Indtast E, O, C, R: ");
    input = Console.ReadLine();
    if (string.IsNullOrEmpty(input)) continue;

    switch (input[0])
    {
        case 'E':
            finish = true;
            break;

        case 'O':
            door.OnDoorOpen();
            break;

        case 'C':
            door.OnDoorClose();
            break;

        case 'R':
            System.Console.WriteLine("Indtast RFID id: ");
            string idString = System.Console.ReadLine();

            int id = Convert.ToInt32(idString);
            rfidReader.OnRfidRead(id);
            break;

        default:
            break;
    }

} while (!finish);
