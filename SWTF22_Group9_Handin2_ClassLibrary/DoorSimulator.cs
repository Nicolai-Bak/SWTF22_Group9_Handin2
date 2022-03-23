﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWTF22_Group9_Handin2_ClassLibrary
{
    public class DoorSimulator : IDoor
    {
        


        public event EventHandler<DoorEventArgs> DoorEvent;

        public DoorStates doorstate { get; set; }

        public void DoorChangedEvent(DoorEventArgs e)
        {
            EventHandler<DoorEventArgs> handler = DoorEvent;

            if (handler != null)
                handler(this, e);

        }

        public DoorSimulator()
        {
            doorstate = DoorStates.DoorClosed;
        }

        public void OnDoorOpen()
        {
            if (doorstate == DoorStates.DoorClosed)
                DoorChangedEvent(new DoorEventArgs(DoorStates.DoorOpen));
                doorstate = DoorStates.DoorOpen;
        }

        public void OnDoorClose()
        {
            if (doorstate == DoorStates.DoorOpen)
                DoorChangedEvent(new DoorEventArgs(DoorStates.DoorClosed));
                doorstate = DoorStates.DoorClosed;
        }

        public void LockDoor()
        {
            if (doorstate == DoorStates.DoorClosed)
                doorstate = DoorStates.DoorLocked;
        }

        public void UnlockDoor()
        {
            if (doorstate == DoorStates.DoorLocked)
                doorstate = DoorStates.DoorClosed;
        }


    }
}