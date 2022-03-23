using System;

namespace SWTF22_Group9_Handin2_ClassLibrary
{
    public class Display : IDisplay
    {

        public void DisplayMsg(string msg)
        {
            Console.WriteLine(msg);
        }
    }

}

