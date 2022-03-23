using NUnit.Framework;
using SWTF22_Group9_Handin2_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace SWTF22_Group9_Handin2.Test.Unit
{
    [TestFixture]
    public class TestDisplay
    {
        private IDisplay _display;
        [SetUp]
        public void Setup()
        {
            _display = Substitute.For<IDisplay>();
        }

        [Test]
        public void Display_DisplayMsg_Prints_String()
        {
            var msg = "Test";
            _display.DisplayMsg(msg);
            _display.Received(1).DisplayMsg(msg);
        }

    }
}