namespace SWTF22_Group9_Handin2_ClassLibrary;

public interface IRfidReader
{
    event EventHandler<RfidEventArgs> RfidEvent;
}