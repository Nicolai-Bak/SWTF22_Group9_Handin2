using NUnit.Framework;
using SWTF22_Group9_Handin2_ClassLibrary;
using System;
using System.IO;

namespace SWTF22_Group9_Handin2.Test.Unit;

[TestFixture]
public class TestDisplay
{
    private DisplaySimulator _uut;
    [SetUp]
    public void Setup()
    {
        _uut = new DisplaySimulator();
    }
    
    [Test]
    public void DisplayMsg_MsgSentToConsole()
    {
        var msg = "Test";

        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        
        _uut.DisplayMsg(msg);
        
        Assert.AreEqual(msg + "\r\n", stringWriter.ToString());
    }
}