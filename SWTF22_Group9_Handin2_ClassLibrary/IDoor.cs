﻿namespace SWTF22_Group9_Handin2_ClassLibrary;

public interface IDoor
{
    public event EventHandler<DoorEventArgs> DoorEvent;

    public void LockDoor();

    public void UnlockDoor();
}