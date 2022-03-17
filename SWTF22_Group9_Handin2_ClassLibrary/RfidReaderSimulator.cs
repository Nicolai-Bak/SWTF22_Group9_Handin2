namespace SWTF22_Group9_Handin2_ClassLibrary;

public class RfidReaderSimulator : IRfidReader
{
    public event EventHandler<RfidEventArgs>? RfidEvent;

    public void OnRfidRead(int id)
    {
        RfidEvent?.Invoke(this, new RfidEventArgs() {Id = id});
    }
}