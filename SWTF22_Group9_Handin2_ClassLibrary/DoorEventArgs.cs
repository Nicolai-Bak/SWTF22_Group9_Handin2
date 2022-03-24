namespace SWTF22_Group9_Handin2_ClassLibrary;
public enum DoorStates
{
    DoorLocked,
    DoorOpen,
    DoorClosed
}

public class DoorEventArgs : EventArgs
{
 
    public DoorStates States { get; set; }

    public DoorEventArgs(DoorStates states)
    {
        States = states;
    }

}